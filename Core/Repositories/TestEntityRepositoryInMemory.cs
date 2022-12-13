using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class TestEntityRepositoryInMemory : ITestDataRepository
    {
        static List<TestEnity> TestEntities = Seed();

        public Task<List<TestEnity>> GetEnities()
        {
            return Task.FromResult(TestEntities);
        }
        static List<TestEnity> Seed()
        {
            List<TestEnity> entities = new List<TestEnity>();
            for (int i = 1; i < 20; i++)
            {
                if (i % 2 == 0)
                    entities.Add(new TestEnity(i, $"ForAdmin-{i}"));
                else
                    entities.Add(new TestEnity(i, $"ForUser-{i}"));
            }
            return entities;

        }
    }
}
