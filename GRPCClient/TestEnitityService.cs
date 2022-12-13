using Core.Entities;
using Core.Services;
using Grpc.Core;
using GrpcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GrpcService.EntityService;

namespace GRPCClient
{
    public class TestEnitityService : ITestEntityService
    {
        private readonly EntityServiceClient entityServiceClient;
        private readonly ITokenProvider tokenProvider;

        public TestEnitityService(EntityServiceClient entityServiceClient, ITokenProvider tokenProvider)
        {
            this.entityServiceClient=entityServiceClient;
            this.tokenProvider=tokenProvider;
        }
        public async Task<List<TestEnity>> GetEnities()
        {
            var stream = entityServiceClient.GetEntities(new Google.Protobuf.WellKnownTypes.Empty(), await GetAuthMetadata());
            List<TestEnity> enities = new();
            await foreach (var item in stream.ResponseStream.ReadAllAsync())
            {
                enities.Add(new TestEnity(item.Id, item.Data));
            }
            return enities;
        }
        private async Task<Metadata> GetAuthMetadata()
        {
            var token = await tokenProvider.GetTokenAsync();
            return new Metadata() { { "Authorization", token } };
        }
    }
}
