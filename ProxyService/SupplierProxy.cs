﻿using Entities.Models;
using ProxyService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProxyService
{
    public class SupplierProxy : ISupplierProxy
    {
        private readonly HttpClient _httpClient;
        public SupplierProxy()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7128/api/Supplier/") // Asegúrate de que esta URL coincida con la configuración de tu servicio
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<Supplier> CreateAsync(Supplier supplier)
        {
            try
            {
                var json = JsonSerializer.Serialize(supplier);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("", content);
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Supplier>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (global::System.Exception ex)
            {
                // Manejar la excepción (e.g., logging)
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{id}");
                return response.IsSuccessStatusCode;
            }
            catch (global::System.Exception ex)
            {
                // Manejar la excepción (e.g., logging)
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Supplier>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Supplier>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                // Manejar la excepción (e.g., logging)
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<Supplier> GetByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{id}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Supplier>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (global::System.Exception ex)
            {
                // Manejar la excepción (e.g., logging)
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateAsync(int id, Supplier supplier)
        {
            try
            {
                var json = JsonSerializer.Serialize(supplier);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{id}", content);
                return response.IsSuccessStatusCode;
            }
            catch (global::System.Exception)
            {
                throw;
            }
        }
    }
}
