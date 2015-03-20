using Autofac;
using RtDiff.Core;
using RtDiff.XmlDiffPatchShim;

namespace RtDiff.Console
{
class Program
{
	private static IContainer Container { get; set; }

	static void Main(string[] args)
	{
		var slaFile = args[0];
		var diffFile = args[1];

		Container = RegisterServices();
		using (var scope = Container.BeginLifetimeScope())
		{
			var watchr = scope.Resolve<IResourceWatcher>();
			var logger = scope.Resolve<IEventLogger>();
			var writer = scope.Resolve<IDiffWriter>();
			
			var comparer = scope.Resolve<IResourceComparer>
				(new TypedParameter(typeof(IEventLogger), logger), 
				 new TypedParameter(typeof(IDiffWriter), writer));

			writer.OutputFile = diffFile;
			comparer.TakeSnapshot(slaFile);
			watchr.FileChanged += comparer.ResourceChanged;
			watchr.StartWatching(slaFile);

			System.Console.WriteLine("Press \'q\' to stop waiting for changes.");
			while(System.Console.Read()!='q');
		}
	}



	private static IContainer RegisterServices()
	{
		var buildr = new ContainerBuilder();

		buildr.RegisterType<WinFsWatcher.FileWatcher>().As<IResourceWatcher>();
		buildr.RegisterType<EventLogger>().As<IEventLogger>();
		buildr.RegisterType<XmlComparer>().As<IResourceComparer>();
		buildr.RegisterType<DiffgramToHtml>().As<IDiffWriter>();
		
		return buildr.Build();
	}

}
}
