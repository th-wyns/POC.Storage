# Introduction 
Statically Linked Source Files

Contains source files that can be linked to projects at compile time,
such as helpers, extension methods, internally used stuffs.

All types should be declared as **internal** to avoid name conflicts
when projects, which add reference to each other, link the same file.
https://github.com/aspnet/AspNetCore/tree/master/src/Shared

Advantages:
    - Helps avoid polluting type system with extensions
    - No extra DLL load - they are statically linked and optimized at compile time
    - Shared internal API code base accross the repository projects