using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GMailLabelCleanup.Core.Models;
using Google.Apis.Gmail.v1.Data;
using GMailLabelCleanup.Core.Components;

namespace GMailLabelCleanup.Core.Managers
{
    public class LabelsManager
    {

        #region Global Variables

        private readonly GmailApiComponent _gmailApiComponent;
        private readonly string _userId;
        private readonly string _userName;

        #endregion

        #region Properties


        #endregion

        #region Constructor

        public LabelsManager(string userId, string userName)
        {
            this._gmailApiComponent = new GmailApiComponent();
            this._userId = userId;
            this._userName = userName;
        }

        #endregion

        #region Methods

        public async Task<LabelInfo> GetAsync(string id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            var gmailService = await _gmailApiComponent.GetGmailService(_userId, _userName);
            var result = await await _gmailApiComponent.ExecuteBackoffAsync(() => gmailService.Users.Labels.Get("me", id).ExecuteAsync().ConfigureAwait(false))
                .ConfigureAwait(false);
            return result.ToLabelInfo();
        }

        public async Task<IReadOnlyList<LabelInfo>> ListAsync()
        {
            var gmailService = await _gmailApiComponent.GetGmailService(_userId, _userName);
            var result = await await _gmailApiComponent.ExecuteBackoffAsync(() => gmailService.Users.Labels.List("me").ExecuteAsync().ConfigureAwait(false))
                .ConfigureAwait(false);

            return result.Labels.Select(o => o.ToLabelInfo())
                .ToList()
                .AsReadOnly();
        }

        public async Task<LabelInfo> CreateAsync(LabelInfo labelInfo)
        {
            if (labelInfo == null)
                throw new ArgumentNullException("labelInfo");

            var body = labelInfo.ToLabel();

            var gmailService = await _gmailApiComponent.GetGmailService(_userId, _userName);
            var result = await await _gmailApiComponent.ExecuteBackoffAsync(() => gmailService.Users.Labels.Create(body, "me").ExecuteAsync().ConfigureAwait(false))
                .ConfigureAwait(false);

            return result.ToLabelInfo();
        }

        public async Task<LabelInfo> UpdateAsync(LabelInfo labelInfo)
        {
            if (labelInfo == null)
                throw new ArgumentNullException("labelInfo");

            var body = labelInfo.ToLabel();

            var gmailService = await _gmailApiComponent.GetGmailService(_userId, _userName);
            var result = await await _gmailApiComponent.ExecuteBackoffAsync(() => gmailService.Users.Labels.Update(body, "me", labelInfo.Id).ExecuteAsync().ConfigureAwait(false))
                .ConfigureAwait(false);

            return result.ToLabelInfo();
        }

        public async Task<string> DeleteAsync(string id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            var gmailService = await _gmailApiComponent.GetGmailService(_userId, _userName);
            var result = await await _gmailApiComponent.ExecuteBackoffAsync(() => gmailService.Users.Labels.Delete("me", id).ExecuteAsync().ConfigureAwait(false))
                .ConfigureAwait(false);

            return result;
        }

        #endregion

    }
}