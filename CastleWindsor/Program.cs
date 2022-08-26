
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace CastleWindsor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Processor processor = new Processor();
            processor.Process();
        }

        interface IMessageSender
        {
            void SendMessage(string message);
        }

        interface ILogger
        {
            void WriteLog(string message);
        }

        class MailSender : IMessageSender
        {
            public void SendMessage(string message)
            {
                Console.WriteLine(String.Format("MailSender: "+message));
            }
        }
        class SmsSender : IMessageSender
        {
            public void SendMessage(string message)
            {
                Console.WriteLine(String.Format("SmsSender: "+message));
            }
        }

        class DBLogger : ILogger
        {
            public void WriteLog(string message)
            {
                Console.WriteLine(String.Format("DBLogger:"+message));
            }
        }

        class FileLogger : ILogger
        {
            public void WriteLog(string message)
            {
                Console.WriteLine(String.Format("FileLogger: "+message));
            }
        }

        public static class IoCUtil
        {
            private static IWindsorContainer _container = null;
            private static IWindsorContainer Container
            {
                get
                {
                    if (_container == null)
                    {
                        _container = BootstrapContainer();
                    }
                    return _container;
                }
            }

            // IoC Container’ı verilen konfigürasyonlara göre yaratan metodumuz
            private static IWindsorContainer BootstrapContainer()
            {
                return new WindsorContainer().Register(
                Component.For<IMessageSender>().ImplementedBy<SmsSender>(),
                Component.For<ILogger>().ImplementedBy<FileLogger>());
            }

            public static T Resolve<T>()
            {
                return Container.Resolve<T>();
            }
        }

        class Processor
        {
            ILogger logger = null;
            IMessageSender messageSender;

            private MailSender mailSender;
            private FileLogger fileLogger;

            public Processor()
            {
                logger = IoCUtil.Resolve<ILogger>();
                messageSender = IoCUtil.Resolve<IMessageSender>();
            }

            public Processor(MailSender mailSender, FileLogger fileLogger)
            {
                this.mailSender = mailSender;
                this.fileLogger = fileLogger;
            }

            public void Process()
            {
                logger.WriteLog("Log Text");
                messageSender.SendMessage("Message Text");
            }
        }
    }
}