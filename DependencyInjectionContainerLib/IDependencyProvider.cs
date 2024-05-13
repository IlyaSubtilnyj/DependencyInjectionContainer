namespace DependencyInjectionContainerLib
{
    public interface IDependencyProvider
    {
        /**
         * Finds an entry of the container by its identifier and returns it.
         */
        public T Resolve<T>();

        /**
         * Returns true if the container can return an entry for the given identifier.
         * Returns false otherwise.
         */
        public bool Has<T>();
    }
}