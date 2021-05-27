using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using System;
using System.IO;
using System.Linq;

namespace AutoFacTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //Aop();
            IocLifeInstancePerDependency();
            IocLifeInstancePerLifetimeScope();
            IocLifeInstancePerRequest();
            //Todo iocEvent

            Console.ReadKey();
        }

        static void Aop()
        {
            var builder = new ContainerBuilder();
            //1 动态注入拦截器
            //启用类代理, 继承接口n, 特性y, 虚方法y
            //builder.RegisterType<Circle>().EnableClassInterceptors();
            //启动接口代理, 继承接口y, 特性n, 虚方法n
            builder.RegisterType<Circle>().As<IShape>()
                //.InterceptedBy(typeof(CallLogger))
                .EnableInterfaceInterceptors();

            //2 注册拦截器到autofac => AOP
            builder.Register(c => new CallLogger(Console.Out));
            builder.Register(c => new CallCache(Console.Out));
            //3 创建容器
            var container = builder.Build();
            //4 从容器获取类型 => IOC
            //var circle = container.Resolve<Circle>();
            var circle = container.Resolve<IShape>();
            //5 调用
            circle.Area(10);
        }

        static void IocLifeInstancePerDependency()
        {
            
            var builder = new ContainerBuilder();
            //InstancePerDependency
            builder.RegisterType<Woker>().InstancePerDependency();
            //！！需要先registerType然后在build container
            var container = builder.Build();

            Console.WriteLine("LifeCycle: InstancePerDependency");
            using (var scope = container.BeginLifetimeScope())
            {
                for (int i = 0; i < 10; i++)
                {
                    var w = scope.Resolve<Woker>();
                    w.Do();
                }
            }
        }

        static void IocLifeInstancePerLifetimeScope()
        {

            var builder = new ContainerBuilder();
            //InstancePerDependency
            builder.RegisterType<Woker>().InstancePerLifetimeScope();
            //！！需要先registerType然后在build container
            var container = builder.Build();

            Console.WriteLine("LifeCycle: InstancePerDependency");
            using (var scope = container.BeginLifetimeScope())
            {
                for (int i = 0; i < 10; i++)
                {
                    var w = scope.Resolve<Woker>();
                    w.Do();
                }
            }
            var c = container.Resolve<Woker>();
            c.Do();
        }

        static void IocLifeInstancePerRequest()
        {

            var builder = new ContainerBuilder();
            //InstancePerDependency
            builder.RegisterType<Woker>().InstancePerRequest();
            //！！需要先registerType然后在build container
            var container = builder.Build();

            Console.WriteLine("LifeCycle: InstancePerRequest");
            using (var scope = container.BeginLifetimeScope())
            {
                for (int i = 0; i < 10; i++)
                {
                    var w = scope.Resolve<Woker>();
                    w.Do();
                }
            }
        }
    }

    public class Woker
    {
        public void Do()
        {
            Console.WriteLine($"woker hashCode {this.GetHashCode()}");
        }
    }


    public interface IShape
    {
        int Area(int a);
    }

    //设置拦截器
    [Intercept(typeof(CallLogger))]
    [Intercept(typeof(CallCache))]
    public class Circle : IShape
    {
        //不继承,必须是虚方法??
        public int Area(int a)
        {
            Console.WriteLine(" you are calling Area method");
            return (int)Math.Pow(a,2);
        }
    }

    public class CallCache : IInterceptor
    {
        TextWriter _output;

        public CallCache(TextWriter output)
        {
            _output = output;
        }

        public void Intercept(IInvocation invocation)
        {
            _output.WriteLine($"Before CallCache {invocation.Method.Name}, " +
                $"Args:{string.Join(",", invocation.Arguments.Select(a => a))}");
            //call method
            invocation.Proceed();
            _output.WriteLine($"After CallCache {invocation.ReturnValue}");
        }
    }

    public class CallLogger : IInterceptor
    {
        TextWriter _output;

        public CallLogger(TextWriter output)
        {
            _output = output;
        }

        public void Intercept(IInvocation invocation)
        {
            _output.WriteLine($"Before CallLogger {invocation.Method.Name}, " +
                $"Args:{string.Join(",", invocation.Arguments.Select(a => a))}");
            //call method
            invocation.Proceed();
            _output.WriteLine($"After CallLogger {invocation.ReturnValue}");
        }
    }
}
