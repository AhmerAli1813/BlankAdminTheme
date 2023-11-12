using DE.Infrastructure.Concept;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DPWVessel.Model.Features.Shared
{
    public class GetStringConstantEnumerationRequest : IRequest<GetStringConstantEnumerationResponse>
    {
        public Type DropdownEnum { get; set; }
    }

    public class GetStringConstantEnumerationResponse : Response
    {
        public List<string> GetData { get; set; }
    }

    public class GetStringConstantEnumerationResquestHandler : IRequestHandler<GetStringConstantEnumerationRequest, GetStringConstantEnumerationResponse>
    {
        public GetStringConstantEnumerationResquestHandler()
        {

        }

        public GetStringConstantEnumerationResponse Execute(GetStringConstantEnumerationRequest request)
        {

            Dictionary<string, string> getdata = new Dictionary<string, string>();
            getdata = GetFieldValues(request.DropdownEnum);

            return new GetStringConstantEnumerationResponse { GetData = getdata.Values.ToList() };
        }
        public static Dictionary<string, string> GetFieldValues(Type type)
        {

            return type
                      .GetFields(BindingFlags.Public | BindingFlags.Static)
                      .Where(f => f.FieldType == typeof(string))
                      .ToDictionary(f => f.Name,
                                    f => (string)f.GetValue(null));
        }
    }
}
