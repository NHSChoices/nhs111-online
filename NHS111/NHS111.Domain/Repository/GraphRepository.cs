﻿using System;
using Neo4jClient;
using NHS111.Domain.Configuration;

namespace NHS111.Domain.Repository
{
    public class GraphRepository : IGraphRepository
    {
        public IGraphClient Client { get; private set; }

        public GraphRepository(IConfiguration configuration)
        {
            var client = new GraphClient(new Uri(configuration.GetGraphDbUrl()));

            client.Connect();
            Client = client;
        }
    }

    public interface IGraphRepository
    {
        IGraphClient Client { get; }
    }
}