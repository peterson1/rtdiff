using Autofac;
using RtDiff.Core;
using RtDiff.XmlDiffLogger;

namespace RtDiff.Console
{
class Program
{
	private static IContainer Container { get; set; }

	static void Main(string[] args)
	{
		var slaFile = args[0];

		Container = RegisterServices();
		using (var scope = Container.BeginLifetimeScope())
		{
			var watchr = scope.Resolve<IResourceWatcher>();
			var writr = scope.Resolve<IOutput>();
			var loggr = scope.Resolve<IDiffLogger>
				(new TypedParameter(typeof(IOutput), writr));

			loggr.TakeSnapshot(slaFile);
			watchr.FileChanged += loggr.HandleChange;
			watchr.StartWatching(slaFile);

			System.Console.WriteLine("Press \'q\' to stop waiting for changes.");
			while(System.Console.Read()!='q');
		}
	}



	private static IContainer RegisterServices()
	{
		var buildr = new ContainerBuilder();

		buildr.RegisterType<WinFsWatcher.FileWatcher>().As<IResourceWatcher>();
		buildr.RegisterType<ConsoleOutput>().As<IOutput>();
		buildr.RegisterType<XmlDiffer>().As<IDiffLogger>();
		
		return buildr.Build();
	}

}
}
