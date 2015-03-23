using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using RtDiff.Tools;

namespace RtDiff.Console.Tools
{
	public class Assembly : System.Reflection.Assembly
	{
		public static Version ExeVersion { get { 
			return System.Reflection.Assembly
				.GetExecutingAssembly().GetName().Version; } }
		
		public static string ExeFile { get { 
			return System.Reflection.Assembly
				.GetExecutingAssembly().Location; } }
		
		public static string ExeFolder { get { 
			return Path.GetDirectoryName(Assembly.ExeFile); } }

		public static string BesideExe(string fileName)	{
			return Path.Combine(Assembly.ExeFolder, fileName); }
		
		public static string CompiledAgo { get 
		{
			//var lastCompile = Directory.GetLastWriteTime(Assembly.ExeFolder);

			var dir = new DirectoryInfo(Assembly.ExeFolder);

			var latest = dir.GetFiles()
						 .OrderByDescending(f => f.LastWriteTime)
						 .First();			

			//return DateTime.Now.Subtract(latest.LastWriteTime).Humanize();
			return latest.LastWriteTime.TimeAgo();

		}}
	}


	public static class AssemblyExtensions
	{


		public static PropertyInfo[] PublicProperties(this Type objType)
		{
			return objType.GetProperties(BindingFlags.Public 
									   | BindingFlags.Instance
									   ).ToArray();
		}


	}


	public class PropertyInfoNameComparer : IEqualityComparer<PropertyInfo>
	{

		public bool Equals(PropertyInfo x, PropertyInfo y)
		{
			if (Object.ReferenceEquals(x, y)) return true;
			if ((x == null) || (y == null)) return false;
			return x.Name == y.Name; 
		}

		public int GetHashCode(PropertyInfo obj)
		{
			if (Object.ReferenceEquals(obj, null)) return 0;
			return obj.Name.GetHashCode(); 
		}
	}
}
