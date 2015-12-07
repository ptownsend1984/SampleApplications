using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GMailLabelCleanup.Core.Attributes.ActionFilters;
using GMailLabelCleanup.Core.Controllers;
using GMailLabelCleanup.Core.Managers;
using GMailLabelCleanup.Core.Models;
using GMailLabelCleanup.Core.ViewModels.Labels;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Gmail.v1;
using Microsoft.ApplicationInsights;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace GMailLabelCleanup.Controllers
{
    [Authorize(Order = 0)]
    public class LabelsController : GlcController
    {

        public LabelsController()
        {
        }

        private LabelsManager GetLabelsManager()
        {
            var userId = GetUserId();
            var userName = GetUserName();

            return new LabelsManager(userId, userName);
        }

        // GET: Labels
        public async Task<ActionResult> Index()
        {
            var labelsManager = GetLabelsManager();
            var labels = await labelsManager.ListAsync();

            await Task.Run(() => new TelemetryClient().TrackEvent("ListLabel"));

            return View(new IndexViewModel
                {
                    Labels = labels
                });
        }

        public ActionResult Create()
        {
            var viewModel = new EditViewModel
            {
                Id = string.Empty,
                Name = string.Empty,
                LabelType = LabelType.User,
                LabelListVisibilityType = (int)LabelListVisibilityType.Show,
                MessageListVisibilityType = (int)MessageListVisibilityType.Show
            };
            SetEditSelectLists(viewModel);

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                SetEditSelectLists(viewModel);
                return View(viewModel);
            }

            var labelInfo = new LabelInfo
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                LabelType = viewModel.LabelType,
                LabelListVisibilityType = (LabelListVisibilityType)viewModel.LabelListVisibilityType,
                MessageListVisibilityType = (MessageListVisibilityType)viewModel.MessageListVisibilityType
            };

            var labelsManager = GetLabelsManager();
            await labelsManager.CreateAsync(labelInfo);

            await Task.Run(() => new TelemetryClient().TrackEvent("CreateLabel"));

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(string id)
        {
            var labelsManager = GetLabelsManager();
            LabelInfo label;
            try
            {
                label = await labelsManager.GetAsync(id);
            }
            catch (GoogleApiException ex)
            {
                if (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                    return HttpNotFound(string.Format("Label id {0} does not exist.", id));
                else
                    throw;
            }

            if (!label.IsUserType)
                return HttpNotFound();

            var viewModel = new EditViewModel
            {
                Id = label.Id,
                Name = label.Name,
                LabelType = label.LabelType,
                LabelListVisibilityType = (int)label.LabelListVisibilityType,
                MessageListVisibilityType = (int)label.MessageListVisibilityType
            };
            SetEditSelectLists(viewModel);

            return View(viewModel);
        }        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                SetEditSelectLists(viewModel);
                return View(viewModel);
            }

            var labelsManager = GetLabelsManager();
            LabelInfo label;
            try
            {
                label = await labelsManager.GetAsync(viewModel.Id);
            }
            catch (GoogleApiException ex)
            {
                if (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                    return HttpNotFound(string.Format("Label id {0} does not exist.", viewModel.Id));
                else
                    throw;
            }

            if (!label.IsUserType)
                return HttpNotFound();

            var labelInfo = new LabelInfo
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                LabelType = viewModel.LabelType,
                LabelListVisibilityType = (LabelListVisibilityType)viewModel.LabelListVisibilityType,
                MessageListVisibilityType = (MessageListVisibilityType)viewModel.MessageListVisibilityType
            };

            await labelsManager.UpdateAsync(labelInfo);

            await Task.Run(() => new TelemetryClient().TrackEvent("EditLabel"));

            return RedirectToAction("Index");
        }

        private void SetEditSelectLists(EditViewModel viewModel)
        {
            var labelListVisibilityTypes = new SelectList(new[]
            {
                new { ID = (int)LabelListVisibilityType.Show, Name = "Show" },
                new { ID = (int)LabelListVisibilityType.Hide, Name = "Hide" },
                new { ID = (int)LabelListVisibilityType.ShowIfUnread, Name = "Show if unread" }                
            }, "ID", "Name");

            var messageListVisibilityTypes = new SelectList(new[]
            {
                new { ID = (int)MessageListVisibilityType.Show, Name = "Show" },
                new { ID = (int)MessageListVisibilityType.Hide, Name = "Hide" }
            }, "ID", "Name");

            viewModel.LabelListVisibilityTypes = labelListVisibilityTypes;
            viewModel.MessageListVisibilityTypes = messageListVisibilityTypes;
        }

        public async Task<ActionResult> Delete(string id)
        {
            var labelsManager = GetLabelsManager();
            LabelInfo label;
            try
            {
                label = await labelsManager.GetAsync(id);
            }
            catch (GoogleApiException ex)
            {
                if (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                    return HttpNotFound(string.Format("Label id {0} does not exist.", id));
                else
                    throw;
            }

            if (!label.IsUserType)
                return HttpNotFound();

            await labelsManager.DeleteAsync(label.Id);

            await Task.Run(() => new TelemetryClient().TrackEvent("DeleteLabel"));

            return RedirectToAction("Index");
        }


    }
}