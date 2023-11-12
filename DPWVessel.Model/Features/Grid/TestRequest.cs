﻿using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;

namespace DPWVessel.Model.Features.Grid
{
    public class TestResponse : Response
    {
        public string Data { get; set; }
        public int ID { get; set; }
    }

    public class TestRequest : IRequest<TestResponse>
    {
        public int Id { get; set; }
    }

    public class TestRequestHandler : IRequestHandler<TestRequest, TestResponse>
    {
        private dpw_VesselEntities _dbContext;
        public TestRequestHandler(dpw_VesselEntities dbContext)
        {
            _dbContext = dbContext;
        }

        public TestResponse Execute(TestRequest request)
        {/*
            var row = _dbContext..FirstOrDefault(x => x.Id == request.Id);
            if (row == null)
            {
                row = new ();
                _dbContext.Zones.Add(row);
            }
            row.Name = "Test Successful " + DateTime.Now.Ticks;*/
            return new TestResponse
            {
                Data = "Test Data",
                ID = 1,
            };
        }
    }

}
