using Core.Models;
using Dal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Providers
{
    public sealed class SettingsProvider
    {
        private static readonly Lazy<SettingsProvider> lazy =
            new Lazy<SettingsProvider>(() => new SettingsProvider());

        public static SettingsProvider Instance { get { return lazy.Value; } }

        public IDictionary<Core.Enums.Settings, SettingsModel> Settings { get; set; }

        private SettingsProvider()
        {
            var rep = new SettingsRepository();
            Settings = rep.GetAll().Select(x => new SettingsModel {
                Id = x.Id,
                Value = x.Value,
                Key = x.Key,
                CreateDate = x.CreateDate
            }).ToDictionary(x=>(Core.Enums.Settings)x.Id);
        }
    }

}
