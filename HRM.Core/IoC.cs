using Spring.Context.Support;
using Spring.Context;
using System.IO;
using System;

namespace HRM.Core
{
		
	public static class IoC
	{
		static readonly IApplicationContext _appContext = new XmlApplicationContext(false, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "spring.cfg.xml"));

		public static T Resolve<T>(string name)
		{
			return (T)_appContext.GetObject(name);
		}

		public static bool Exists(string name)
		{
			return _appContext.ContainsObjectDefinition(name);
		}
	}
	
	
}
