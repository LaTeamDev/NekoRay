using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FreeTypeSharp;
using HarfBuzzSharp;
using static FreeTypeSharp.FT;
using static FreeTypeSharp.FT_LOAD;
using static FreeTypeSharp.FT_Render_Mode_;
using static WebRenderingTest.FreeType;

namespace WebRenderingTest;

file static unsafe class FreeType {
    public static void ThrowIfError(this FT_Error error) {
        if (error == FT_Error.FT_Err_Ok)
            return;
        throw new FreeTypeException(error);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr FloatToFixed(float value) {
        var aligned = value * 65536;
        return *(IntPtr*)&aligned;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float FixedToFloat(IntPtr value) {
        var aligned = value / 65536;
        return *(float*)&aligned;
    }
}

public unsafe class LibraryRec : IDisposable {
    public FT_LibraryRec_* _ptr;

    public LibraryRec() {
        fixed (FT_LibraryRec_** lib_ptr = &_ptr)
            FT_Init_FreeType(lib_ptr).ThrowIfError();
    }
    public void Dispose() {
        FT_Done_Library(_ptr);
    }
}

public unsafe class FaceRec : IDisposable {
    internal FT_FaceRec_* _face;

    public int GlyphCount => (int) _face->num_glyphs;
    public FT_FACE_FLAG FaceFlags => (FT_FACE_FLAG) _face->face_flags;
    public int FaceIndex => (int) _face->face_index;
    public ushort UnitsPerEm => _face->units_per_EM;
    public int FixedSizeCount => _face->num_fixed_sizes;
    public FT_CharMapRec_* Charmap => _face->charmap;
    public GlyphSlotRec Glyph { get; }

    public FaceRec(LibraryRec lib, string fontName, int faceIndex) {
        fixed (FT_FaceRec_** face_ptr = &_face)
            FT_New_Face(lib._ptr, (byte*)Marshal.StringToHGlobalAnsi(fontName), faceIndex, face_ptr).ThrowIfError();
        Glyph = new GlyphSlotRec(this);
    }
    
    public FaceRec(LibraryRec lib, Span<byte> buffer, int faceIndex) {
        fixed (byte* buffer_ptr = buffer) fixed (FT_FaceRec_** face_ptr = &_face)
            FT_New_Memory_Face(lib._ptr, buffer_ptr, buffer.Length, faceIndex, face_ptr).ThrowIfError();
    }

    public void SetCharSize(int charWidth, int charHeight, uint horizontalResolution, uint verticalResolution) =>
        FT_Set_Char_Size(_face, charWidth, charHeight, horizontalResolution, verticalResolution).ThrowIfError();

    public void SetPixelSizes(uint width, uint height) =>
        FT_Set_Pixel_Sizes(_face, width, height).ThrowIfError();    
    
    public uint GetCharIndex(uint charCode) =>
        FT_Get_Char_Index(_face, charCode);
    
    public void LoadGlyph(uint glyphIndex, FT_LOAD loadFlags) =>
        FT_Load_Glyph(_face, glyphIndex, loadFlags).ThrowIfError();

    public void RenderGlyph(FT_Render_Mode_ renderMode) =>
        FT_Render_Glyph(_face->glyph, FT_RENDER_MODE_NORMAL).ThrowIfError();

    public void SelectCharmap(FT_Encoding_ encoding) =>
        FT_Select_Charmap(_face, encoding).ThrowIfError();

    public void SetTransform(Matrix3x2 transform) {
        var matrix = new FT_Matrix_ {
            xx = FloatToFixed(transform.M11),
            xy = FloatToFixed(transform.M12),
            yx = FloatToFixed(transform.M21),
            yy = FloatToFixed(transform.M22)
        };
        var vector = new FT_Vector_ {
            x = FloatToFixed(transform.M31),
            y = FloatToFixed(transform.M32)
        };
        FT_Set_Transform(_face, &matrix, &vector);
    }

    public void LoadChar(char charCode, FT_LOAD loadFlags) =>
        FT_Load_Char(_face, charCode, loadFlags).ThrowIfError();

    public void Dispose() {
        FT_Done_Face(_face);
    }
}

public unsafe class GlyphSlotRec {
    private FaceRec _faceObj;
    private FT_GlyphSlotRec_* Glyph => _faceObj._face->glyph;
    public FT_Glyph_Format_ Format => Glyph->format;
    public FT_Bitmap_ Bitmap => Glyph->bitmap;
    public int BitmapLeft => Glyph->bitmap_left;
    public int BitmapTop => Glyph->bitmap_top;
    public IntPtr AdvanceX => Glyph->advance.x;
    public IntPtr AdvanceY => Glyph->advance.y;
    public Vector2 AdvanceF => new Vector2(FixedToFloat(AdvanceX), FixedToFloat(AdvanceY));
    
    internal GlyphSlotRec(FaceRec faceObj) {
        _faceObj = faceObj;
    }
}
/*
public unsafe static class HarfBuzzFreeTypeCompat {
    private struct HbFtFont : IDisposable
    {
        public FT_LOAD LoadFlags;
        public bool Symbol;
        public bool Unref;
        public bool Transform;

        public Mutex Lock;
        public FaceRec FtFace;
        public uint CachedSerial;
        // advance_cache;
        public HbFtFont(FaceRec ftFace, bool symbol, bool unref) {
            Lock.WaitOne();
            FtFace = ftFace;
            Symbol = symbol;
            Unref = unref;

            LoadFlags = FT_LOAD_DEFAULT | FT_LOAD_NO_HINTING;

            //CachedSerial = (unsigned) -1;
            //new (&ft_font->advance_cache) hb_ft_advance_cache_t;
            Lock.ReleaseMutex();
        }

        public void Dispose() {
            FtFace.Dispose();
        }
    };
    public static Face CreateFace(FaceRec ftFace, ReleaseDelegate destroy) {
        Face face;

        if ((IntPtr)ftFace._face->stream->read == 0) {
            throw new Exception();
        }
        using var blob = new Blob(
            (IntPtr)ftFace._face->stream->_base,
            (int)   ftFace._face->stream->size,
            MemoryMode.ReadOnly, destroy);
        
        face = new Face(blob, ftFace.FaceIndex);

        face.Index = ftFace.FaceIndex;
        face.UnitsPerEm = ftFace.UnitsPerEm;

        return face;
    }
    
    public static Font CreateFont(FaceRec ftFace, ReleaseDelegate destroy)
    {
        using var face = CreateFace(ftFace, destroy);
        var font = new Font(face);
        SetFuncs(font, ftFace, false);
        hb_ft_font_changed (font);
        return font;
    }

    private static NominalGlyphDelegate GetNominalGlyph =
        (Font font, object fontData, uint unicode, out uint glyph) => {
            glyph = 0;
            var ftFont = (HbFtFont) fontData;
            ftFont.Lock.WaitOne();
            var g = ftFont.FtFace.GetCharIndex(unicode);

            if (g != 0) return true;
            if (!ftFont.Symbol)
                return false;
            switch (font.Face.Table.OS2.GetFontPage()) {
                case FontPage:
                    if (unicode <= 0x00FF) {
                        // For symbol-encoded OpenType fonts, we duplicate the
                        // U+F000..F0FF range at U+0000..U+00FF.  That's what
                        // Windows seems to do, and that's hinted about at:
                        // https://docs.microsoft.com/en-us/typography/opentype/spec/recom
                        // under "Non-Standard (Symbol) Fonts".
                        g = FreeType.FT_Get_Char_Index(ftFont.FtFace, 0xF000u + unicode);
                    }

                    break;
                case HbOt.OS2.FontPageT.FontPageSimpArabic:
                    g = FreeType.FT_Get_Char_Index(ftFont.FtFace, HbArabicPuaSimpMap(unicode));
                    break;
                case HbOt.OS2.FontPageT.FontPageTradArabic:
                    g = FreeType.FT_Get_Char_Index(ftFont.FtFace, HbArabicPuaTradMap(unicode));
                    break;
                default:
                    break;
            }

            if (g == 0)
                return false;

            glyph = g;
            ftFont.Lock.ReleaseMutex();
            return true;
        };

    public static Lazy<FontFunctions> FontFuncs = new(() => {
        FontFunctions funcs = new();
        funcs.SetNominalGlyphDelegate(GetNominalGlyph, );
        hb_font_funcs_set_nominal_glyph_func(funcs, hb_ft_get_nominal_glyph, nullptr, nullptr);
        hb_font_funcs_set_nominal_glyphs_func(funcs, hb_ft_get_nominal_glyphs, nullptr, nullptr);
        hb_font_funcs_set_variation_glyph_func(funcs, hb_ft_get_variation_glyph, nullptr, nullptr);

        hb_font_funcs_set_font_h_extents_func(funcs, hb_ft_get_font_h_extents, nullptr, nullptr);
        hb_font_funcs_set_glyph_h_advances_func(funcs, hb_ft_get_glyph_h_advances, nullptr, nullptr);
        //hb_font_funcs_set_glyph_h_origin_func (funcs, hb_ft_get_glyph_h_origin, nullptr, nullptr);

#ifndef HB_NO_VERTICAL
        //hb_font_funcs_set_font_v_extents_func (funcs, hb_ft_get_font_v_extents, nullptr, nullptr);
        hb_font_funcs_set_glyph_v_advance_func(funcs, hb_ft_get_glyph_v_advance, nullptr, nullptr);
        hb_font_funcs_set_glyph_v_origin_func(funcs, hb_ft_get_glyph_v_origin, nullptr, nullptr);
#endif

#ifndef HB_NO_OT_SHAPE_FALLBACK
        hb_font_funcs_set_glyph_h_kerning_func(funcs, hb_ft_get_glyph_h_kerning, nullptr, nullptr);
#endif
        //hb_font_funcs_set_glyph_v_kerning_func (funcs, hb_ft_get_glyph_v_kerning, nullptr, nullptr);
        hb_font_funcs_set_glyph_extents_func(funcs, hb_ft_get_glyph_extents, nullptr, nullptr);
        hb_font_funcs_set_glyph_contour_point_func(funcs, hb_ft_get_glyph_contour_point, nullptr, nullptr);
        hb_font_funcs_set_glyph_name_func(funcs, hb_ft_get_glyph_name, nullptr, nullptr);
        hb_font_funcs_set_glyph_from_name_func(funcs, hb_ft_get_glyph_from_name, nullptr, nullptr);

#ifndef HB_NO_DRAW
        hb_font_funcs_set_draw_glyph_func(funcs, hb_ft_draw_glyph, nullptr, nullptr);
#endif

#ifndef HB_NO_PAINT
#if (FREETYPE_MAJOR*10000 + FREETYPE_MINOR*100 + FREETYPE_PATCH) >= 21300
    hb_font_funcs_set_paint_glyph_func (funcs, hb_ft_paint_glyph, nullptr, nullptr);
#endif
#endif

        hb_font_funcs_make_immutable(funcs);

        hb_atexit(free_static_ft_funcs);

        return funcs;
    });

    private static void SetFuncs(Font font, FaceRec ftFace, bool unref)
    {
        var symbol = ftFace._face->charmap is not null && ftFace._face->charmap->encoding == FT_Encoding_.FT_ENCODING_MS_SYMBOL;

        var ftFont = new HbFtFont(ftFace, symbol, unref);
        
        font.SetFontFunctions(
            _hb_ft_get_font_funcs(),
            ftFont,
            () => {
                if (ftFont.Unref)
                    ftFont.FtFace.Dispose();

                ftFont.Lock.Dispose();
            });
    }
}*/