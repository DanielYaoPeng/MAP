using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Engine;
using MD.ApkMAP.IRepository;
using MD.ApkMAP.IServices;
using MD.ApkMAP.Repository;
using MD.ApkMAP.Services;

namespace MD.ApkMAP
{
    //public class DependencyRegistrar: IDependencyRegistrar
    //{

    //    public int Order => 1;

    //    public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
    //    {
    //        //泛型类型的注册
    //        //builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IRepository<>)).SingleInstance();

    //        builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>().InstancePerDependency();
    //        builder.RegisterType<AdvertisementRepository>().As<IAdvertisementRepository>().InstancePerDependency();
    //    }
    //}
}
