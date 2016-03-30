using System;
using System.Linq;
using Xunit;

namespace BuildTimesData.Tests
{
    public class LoadingBuildTimesFromAJsonFile
    {
	    private readonly BuildTimes _buildTimes;

	    public LoadingBuildTimesFromAJsonFile()
	    {
		    _buildTimes = new BuildTimes("test-buildtimes.json");
		}

		[Fact]
	    public void CorrectNumberOfBuildsAreIdentified()
	    {
		    Assert.Equal(24, _buildTimes.SolutionBuilds.Count());
	    }

	    [Fact]
	    public void FirstBuildIsDocumentProduction()
	    {
		    Assert.Equal("DocumentProduction", _buildTimes.SolutionBuilds.First().Solution.Name);
	    }

	    [Fact]
	    public void FirstBuildStartTimeIsHalfPastTen()
	    {
		    var expectedDate = DateTime.Parse("2016-03-30T10:32:49.8852898+01:00");
		    Assert.Equal(expectedDate, _buildTimes.SolutionBuilds.First().Start);
	    }

	    [Fact]
	    public void FirstBuildLengthIsNearlySevenSeconds()
	    {
		    Assert.Equal(6994, _buildTimes.SolutionBuilds.First().Time);
	    }

	    [Fact]
	    public void SecondBuildHasFourProjectBuilds()
	    {
		    var secondBuild = _buildTimes.SolutionBuilds.Skip(1).First();
		    Assert.Equal(4, secondBuild.Projects.Count());
	    }

	    [Fact]
	    public void SecondBuildIncludesABuildOfUnitTests()
	    {
		    var secondBuild = _buildTimes.SolutionBuilds.Skip(1).First();
		    Assert.Equal(1, secondBuild.Projects.Count(p => p.Project.Name.EndsWith("UnitTests")));
	    }

	    [Fact]
	    public void DocProductionProjectHasTheSameIdInEveryBuild()
	    {
		    var docProductionBuilds = _buildTimes.SolutionBuilds
													.SelectMany(s => s.Projects)
													.Where(p => p.Project.Name == "Solicitors.DocumentProduction")
													.ToList();
		    Assert.True(docProductionBuilds.All(p => p.Project.Id == docProductionBuilds.First().Project.Id));
	    }

	    [Fact]
	    public void SecondBuildTakesLongerThanItsIndividualProjectBuilds()
	    {
		    var secondBuild = _buildTimes.SolutionBuilds.Skip(1).First();
			Assert.True(secondBuild.Time > secondBuild.Projects.Sum(p => p.Time));
	    }

	    [Fact]
	    public void SecondBuildStartsBeforeItsProjectsStartBuilding()
	    {
		    var secondBuild = _buildTimes.SolutionBuilds.Skip(1).First();
			Assert.True(secondBuild.Projects.All(p => p.Start > secondBuild.Start));
		}
	}
}
