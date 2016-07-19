using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Microsoft.Practices.Unity;

namespace WebAPI.Library.DependencyInjection
{
    public class UnityResolver : IDependencyResolver
    {
        private readonly IUnityContainer mUnityContainer;

        public UnityResolver(IUnityContainer aUnityContainer)
        {
            mUnityContainer = aUnityContainer;
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            mUnityContainer.Dispose();
        }

        #endregion

        #region Implementation of IDependencyScope

        public object GetService(Type serviceType)
        {
            try
            {
                return mUnityContainer.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return mUnityContainer.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        public IDependencyScope BeginScope()
        {
            return new UnityResolver(mUnityContainer.CreateChildContainer());
        }

        #endregion
    }
}