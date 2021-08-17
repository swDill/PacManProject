using System;

namespace PacMan.Utility.DependencyInjection
{
    /*
     * A Injectable attribute to mark fields in a class eligible to be injected with Dependency references.
     * The way this is setup, injectables need to be public as C# reflection cannot see private fields on non-base classes.
     */
    [AttributeUsage(AttributeTargets.Field)]
    public class InjectableAttribute : Attribute
    {
    }
}