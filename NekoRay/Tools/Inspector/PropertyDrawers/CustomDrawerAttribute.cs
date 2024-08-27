using JetBrains.Annotations;

namespace NekoRay.Tools;

[MeansImplicitUse]
public class CustomDrawerAttribute(Type drawerType) : Attribute {
    public Type DrawerType = drawerType;
}