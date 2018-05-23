﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Business.Configuration;
using NHS111.Models.Models.Business.Location;
using RestSharp;

namespace NHS111.Business.Services
{
    public class LocationService: ILocationService
    {
        private readonly IRestClient _restidealPostcodesApi;
        private readonly IConfiguration _configuration;

        public LocationService(IRestClient restidealPostcodesApi, IConfiguration configuration)
        {
            _restidealPostcodesApi = restidealPostcodesApi;
            _configuration = configuration;
        }

        public async Task<List<PostcodeLocationResult>> FindPostcodes(double longitude, double latitude)
        {
            var response = await _restidealPostcodesApi.ExecuteTaskAsync<LocationServiceResult<PostcodeLocationResult>>(
                new RestRequest(_configuration.GetLocationPostcodebyGeoUrl(longitude, latitude), Method.GET));

            if(response.ResponseStatus == ResponseStatus.Completed)
                return response.Data.Result.ToList();
            throw response.ErrorException;
        }

        public async Task<List<AddressLocationResult>> FindAddresses(double longitude, double latitude)
        {
            var postcodes = await FindPostcodes(longitude, latitude);
            if(postcodes.Count > 0)
                return await FindAddresses(postcodes.First().PostCode);
            return new List<AddressLocationResult>();
        }

        public async Task<LocationServiceResult<AddressLocationResult>> ValidateAndFindAddresses(string postcode)
        {
            var response = await _restidealPostcodesApi.ExecuteTaskAsync<LocationServiceResult<AddressLocationResult>>(
                new RestRequest(_configuration.GetLocationByPostcodeUrl(postcode), Method.GET));
            if (response.ResponseStatus == ResponseStatus.Completed)
                return response.Data;
            throw response.ErrorException;
        }
        public async Task<List<AddressLocationResult>> FindAddresses(string postcode)
        {
            var response = await _restidealPostcodesApi.ExecuteTaskAsync<LocationServiceResult<AddressLocationResult>>(
                new RestRequest(_configuration.GetLocationByPostcodeUrl(postcode), Method.GET));
            if (response.ResponseStatus == ResponseStatus.Completed)
                return response.Data.Result.ToList();
            throw response.ErrorException;
        }
    }
     public interface ILocationService
    {
         Task<List<PostcodeLocationResult>> FindPostcodes(double longitude, double latitude);
         Task<List<AddressLocationResult>> FindAddresses(double longitude, double latitude);
         Task<List<AddressLocationResult>> FindAddresses(string postcode);
        Task<LocationServiceResult<AddressLocationResult>> ValidateAndFindAddresses(string postcode);
    }
}
