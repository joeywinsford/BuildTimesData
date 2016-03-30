using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace BuildTimesData
{
	public class BuildTimes
	{
		private readonly string _filePath;

		public BuildTimes(string filePath)
		{
			_filePath = filePath;

			using (var reader = new StreamReader(_filePath))
			{
				var fileContents = reader.ReadToEnd();
				SolutionBuilds = JsonConvert.DeserializeObject<IEnumerable<SolutionBuild>>(fileContents);
			}
		}

		public IEnumerable<SolutionBuild> SolutionBuilds { get; private set; }
	}

	public class SolutionBuild
	{
		public Solution Solution { get; set; }
		public DateTime Start { get; set; }
		public long Time { get; set; }
		public IEnumerable<ProjectBuild> Projects { get; set; }
	}

	public class ProjectBuild	
	{
		public Project Project { get; set; }
		public long Time { get; set; }
		public DateTime Start { get; set; }
	}

	public class Project
	{
		public string Name { get; set; }
		public Guid Id { get; set; }
	}

	public class Solution
	{
		public string Name { get; set; }
	}
}