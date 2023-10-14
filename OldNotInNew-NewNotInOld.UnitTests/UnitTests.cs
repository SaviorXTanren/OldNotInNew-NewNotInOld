using Xunit;

namespace OldNotInNew_NewNotInOld.UnitTests
{
    public class UnitTests
    {
        [Fact]
        public void Basic()
        {
            var results = Run("Samples\\Basic");
            Assert.NotNull(results);

            var oldNotInNew = results.Item1;
            Assert.NotNull(oldNotInNew);
            Assert.NotEmpty(oldNotInNew);
            Assert.Equal(1, oldNotInNew.Count);
            Assert.Equal("file 2.txt", oldNotInNew[0]);

            var newNotInOld = results.Item2;
            Assert.NotNull(newNotInOld);
            Assert.NotEmpty(newNotInOld);
            Assert.Equal(2, newNotInOld.Count);
            Assert.Equal("file 2.txt", newNotInOld[0]);
            Assert.Equal("file 3.txt", newNotInOld[1]);
        }

        [Fact]
        public void InvalidFilePaths()
        {
            var results = Run("Samples\\DOES NOT EXIST");
            Assert.Null(results);
        }

        [Fact]
        public void Empty()
        {
            var results = Run("Samples\\Empty");
            Assert.NotNull(results);

            var oldNotInNew = results.Item1;
            Assert.NotNull(oldNotInNew);
            Assert.Empty(oldNotInNew);

            var newNotInOld = results.Item2;
            Assert.NotNull(newNotInOld);
            Assert.Empty(newNotInOld);
        }

        [Fact]
        public void InvalidHashCharacters()
        {
            var results = Run("Samples\\Invalid Hash Characters");
            Assert.Null(results);
        }

        [Fact]
        public void InvalidHashLength()
        {
            var results = Run("Samples\\Invalid Hash Length");
            Assert.Null(results);
        }

        [Fact]
        public void MultipleFilesWithSameHash()
        {
            var results = Run("Samples\\Multiple Files With Same Hash");
            Assert.NotNull(results);

            var oldNotInNew = results.Item1;
            Assert.NotNull(oldNotInNew);
            Assert.NotEmpty(oldNotInNew);
            Assert.Equal(1, oldNotInNew.Count);
            Assert.Equal("file 2.txt", oldNotInNew[0]);

            var newNotInOld = results.Item2;
            Assert.NotNull(newNotInOld);
            Assert.NotEmpty(newNotInOld);
            Assert.Equal(3, newNotInOld.Count);
            Assert.Equal("file 2.txt", newNotInOld[0]);
            Assert.Equal("file 10.txt", newNotInOld[1]);
            Assert.Equal("file 3.txt", newNotInOld[2]);
        }

        [Fact]
        public void LargeFilesNoDifferences()
        {
            var results = Run("Samples\\Large Files - No Differences");
            Assert.NotNull(results);

            var oldNotInNew = results.Item1;
            Assert.NotNull(oldNotInNew);
            Assert.Empty(oldNotInNew);

            var newNotInOld = results.Item2;
            Assert.NotNull(newNotInOld);
            Assert.Empty(newNotInOld);
        }

        [Fact]
        public void LargeFiles1Difference()
        {
            var results = Run("Samples\\Large Files - 1 Difference");
            Assert.NotNull(results);

            var oldNotInNew = results.Item1;
            Assert.NotNull(oldNotInNew);
            Assert.Empty(oldNotInNew);

            var newNotInOld = results.Item2;
            Assert.NotNull(newNotInOld);
            Assert.NotEmpty(newNotInOld);
            Assert.Equal(1, newNotInOld.Count);
            Assert.Equal("file 2.txt", newNotInOld[0]);
        }

        private Tuple<List<string>, List<string>> Run(string directoryPath)
        {
            return Program.ProcessOldAndNewFiles(Path.Combine(directoryPath, Program.OldShaInputFilename), Path.Combine(directoryPath, Program.NewShaInputFileName));
        }
    }
}