﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Mine.Models;
using Mine.Views;

namespace Mine.ViewModels
{
    public class ItemsIndexViewModel : BaseViewModel
    {
        public ObservableCollection<ItemModel> DataSet { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsIndexViewModel()
        {
            Title = "Items";
            DataSet = new ObservableCollection<ItemModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<ItemCreatePage, ItemModel>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as ItemModel;
                DataSet.Add(newItem);
                await DataStore.CreateAsync(newItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                DataSet.Clear();
                var items = await DataStore.IndexAsync(true);
                foreach (var item in items)
                {
                    DataSet.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Read an item from the datastore
        /// </summary>
        /// <param name="id">ID of the Record</param>
        /// <returns>The Record from ReadAsync</returns>
        public async Task<ItemModel> Read(string id)
        {
            var result = await DataStore.ReadAsync(id);

            return result;
        }

        /// <summary>
        /// Delete th4e record from the system
        /// </summary>
        /// <param name="data">The Record to Delete</param>
        /// <returns>Ture if Deleted</returns>
        public async Task<bool> DeleteAsyc (ItemModel data)
        {
            // Check if the record exists, if it does not, then null is returned
            var record = await Read(data.Id);
            if (record == null)
            {
                return false;
            }

            // Remove from the local data set cache
            DataSet.Remove(data);

            // Call to remove it from teh Data Store
            var result = await DataStore.DeleteAsync(data.Id);

            return result;
        }
    }
}