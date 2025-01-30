import("NekoLib.Filesystem", "NekoLib.Filesystem")

package.path = "lua/?.lua;lua/?/init.lua"

function package.searchpath(name, path, sep, dirsep)
    sep = sep or '.'
    dirsep = dirsep or '/'
    local modname = name:gsub('%.', dirsep)
    for template in path:gmatch("[^;]+") do
        local filename = template:gsub("%?", modname)
        if Files.FileExists(filename) then
            return filename
        end
    end
    return nil, "no file '" .. path:gsub(";", "'\n\tno file '") .. "'"
end

local function searcher_Lua(name)
    local filename, err = package.searchpath(name, package.path, '.', '/')
    if not filename then return err end
    local loader, errmsg = load(Files.GetFile(filename):Read(), filename);
    if not loader then
        return nil, string.format("error loading file '%s':\n\t%s", filename, errmsg)
    end
    return loader, filename
end

local searcher_Preload = package.searchers[1];


package.searchers = {
    searcher_Preload,
    searcher_Lua
}

function package.loadlib(path, init)
    return nil, "dynamic libraries not enabled", "absent"
end
