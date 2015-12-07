using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using GMailLabelCleanup.Data;
using GMailLabelCleanup.Core.ViewModels.Filters;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.ApplicationInsights;
using System.Threading.Tasks;
using GMailLabelCleanup.Core.Controllers;
using MongoDB.Driver;
using System.Configuration;
using GMailLabelCleanup.Data.Documents.Filters;
using MongoDB.Bson;
using GMailLabelCleanup.Core.Components;
using GMailLabelCleanup.Common.Extensions;
using GMailLabelCleanup.Data.Models.Filters;
using System.Data.Entity;

namespace GMailLabelCleanup.Controllers
{
    [Authorize(Order = 0)]
    public class FiltersController : GlcController
    {

        public FiltersController()
        {
        }

        public ActionResult Index()
        {
            var userId = GetUserId();
            var context = this.DbContext;

            var userFilters = context.Filters
                .Where(o => o.UserId == userId)
                .ToList();

            var viewModel = new IndexViewModel
            {
                Filters = userFilters
            };
            return View(viewModel);
        }

        public async Task<ActionResult> Export()
        {
            var userId = GetUserId();
            var context = this.DbContext;

            var filters = context.Filters
                .Include(o => o.FilterProperties)
                .Where(o => o.UserId == userId)
                .ToArray();

            if (!filters.Any())
                return RedirectToAction("Index");

            var filterComponent = new MessageFilterComponent();

            var document = filterComponent.ToXml(filters);

            var stream = new System.IO.MemoryStream();
            using (var writer = System.Xml.XmlWriter.Create(stream, new System.Xml.XmlWriterSettings
            {
                OmitXmlDeclaration = false,
                Indent = true
            }))
            {
                document.WriteTo(writer);
            }
            stream.Position = 0;

            await Task.Run(() => new TelemetryClient().TrackEvent("ExportFilter"));

            return File(stream, "text/xml", string.Format("Export_{0}.xml", DateTime.UtcNow.ToFileTime()));
        }

        public ActionResult Create()
        {
            var viewModel = new EditViewModel();
            InitializeEditViewModel(viewModel, null);
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EditViewModel viewModel)
        {
            return await EditPostCore(viewModel);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var userId = GetUserId();
            var context = this.DbContext;

            var filter = await context.Filters
                .Include(o => o.FilterProperties)
                .Where(o => o.FilterId == id && o.UserId == userId)
                .FirstOrDefaultAsync();
            if (filter == null)
                return HttpNotFound("Could not find filter.");

            var viewModel = new EditViewModel();
            InitializeEditViewModel(viewModel, filter);
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditViewModel viewModel)
        {
            return await EditPostCore(viewModel);
        }

        private void InitializeEditViewModel(EditViewModel viewModel, GMailLabelCleanup.Data.Models.Filters.Filter filter)
        {
            System.Diagnostics.Debug.Assert(viewModel != null);

            viewModel.Id = filter != null ? filter.FilterId : 0;
            viewModel.Description = filter != null ? filter.Description : string.Empty;

            InitializeEditSelectLists(viewModel);
            InitializeEditProperties(viewModel, filter != null ? filter.FilterProperties : new FilterProperty[] { });
        }
        private void InitializeEditSelectLists(EditViewModel viewModel)
        {
            System.Diagnostics.Debug.Assert(viewModel != null);

            var smartLabels = new SelectList(new[]
            {
                new { ID = (int)SmartLabelType.Personal, Name = "Personal" },
                new { ID = (int)SmartLabelType.Social, Name = "Social" },
                new { ID = (int)SmartLabelType.Promotions, Name = "Promotions" },
                new { ID = (int)SmartLabelType.Notification, Name = "Updates" },
                new { ID = (int)SmartLabelType.Group, Name = "Forums" }
            }, "ID", "Name");
            var sizeUnits = new SelectList(new[]
            {
                new { ID = (int)SizeUnitType.MB, Name = "MB" },
                new { ID = (int)SizeUnitType.KB, Name = "KB" },
                new { ID = (int)SizeUnitType.Byte, Name = "Bytes" }
            }, "ID", "Name");
            var sizeOperators = new SelectList(new[]
            {
                new { ID = (int)SizeOperatorType.GreaterThan, Name = "Greater than" },
                new { ID = (int)SizeOperatorType.LessThan, Name = "Less than" }
            }, "ID", "Name");

            viewModel.SmartLabels = smartLabels;
            viewModel.SizeOperators = sizeOperators;
            viewModel.SizeUnits = sizeUnits;
        }
        private void InitializeEditProperties(EditViewModel viewModel, IList<FilterProperty> properties)
        {
            System.Diagnostics.Debug.Assert(viewModel != null);
            System.Diagnostics.Debug.Assert(properties != null);

            var component = new MessageFilterComponent();

            viewModel.CriteriaProperties = new List<EditPropertyViewModel>();
            viewModel.ActionProperties = new List<EditPropertyViewModel>();

            foreach (var pair in MessageFilterComponent.CriteriaPropertyNames)
            {
                var existingProperty = properties
                    .FirstOrDefault(o => o.Name == pair.Value);

                viewModel.CriteriaProperties.Add(CreateEditPropertyViewModel(component, existingProperty, pair.Value));
            }

            foreach (var pair in MessageFilterComponent.ActionPropertyNames)
            {
                var existingProperty = properties
                    .FirstOrDefault(o => o.Name == pair.Value);

                viewModel.ActionProperties.Add(CreateEditPropertyViewModel(component, existingProperty, pair.Value));
            }
        }
        private EditPropertyViewModel CreateEditPropertyViewModel(
            MessageFilterComponent component,
            FilterProperty existingProperty,
            string name
            )
        {
            System.Diagnostics.Debug.Assert(component != null);

            var isCheckType = component.GetIsBooleanProperty(name);

            return new EditPropertyViewModel
            {
                IsIncluded = existingProperty != null && existingProperty.IsIncluded,
                Id = existingProperty != null ? (int?)existingProperty.FilterPropertyId : null,
                Timestamp = existingProperty != null ? existingProperty.Timestamp : null,
                Name = name,
                Value = existingProperty != null ? existingProperty.Value : string.Empty,
                IsCheckType = isCheckType,
                IsChecked = isCheckType && existingProperty != null ? string.Equals(existingProperty.Value, "true", StringComparison.OrdinalIgnoreCase) : false
            };
        }

        private async Task<ActionResult> EditPostCore(EditViewModel viewModel)
        {
            System.Diagnostics.Debug.Assert(viewModel != null);

            ValidateProperties(viewModel);
            if (!ModelState.IsValid)
            {
                InitializeEditSelectLists(viewModel);
                return View(viewModel);
            }

            var userId = GetUserId();
            var context = this.DbContext;

            var isNew = viewModel.Id == 0;

            GMailLabelCleanup.Data.Models.Filters.Filter filter;
            if (!isNew)
            {
                filter = await context.Filters
                    .Include(o => o.FilterProperties)
                    .Where(o => o.FilterId == viewModel.Id && o.UserId == userId)
                    .FirstOrDefaultAsync();

                if (filter == null)
                    return HttpNotFound("Could not find filter.");
            }
            else
            {
                filter = new GMailLabelCleanup.Data.Models.Filters.Filter
                {
                    UserId = userId,
                    DateCreatedUtc = DateTime.UtcNow,
                    Description = viewModel.Description,
                    FilterProperties = new List<FilterProperty>()
                };
            }

            if (isNew)
            {
                context.Filters.Add(filter);
            }

            if (ApplyEdits(viewModel, filter) || isNew)
            {
                await context.SaveChangesAsync();

                if (isNew)
                {
                    await Task.Run(() => new TelemetryClient().TrackEvent("CreateFilter"));
                }
                else
                {
                    await Task.Run(() => new TelemetryClient().TrackEvent("EditFilter"));
                }
            }

            return RedirectToAction("Index");
        }
        private void ValidateProperties(EditViewModel viewModel)
        {
            System.Diagnostics.Debug.Assert(viewModel != null);

            if (!viewModel.CriteriaProperties.Any(o => o.IsIncluded))
            {
                ModelState.AddModelError("CriteriaProperties", "At least one criteria must be applied.");
            }
            if (!viewModel.ActionProperties.Any(o => o.IsIncluded))
            {
                ModelState.AddModelError("ActionProperties", "At least one action must be applied.");
            }
        }
        private bool ApplyEdits(EditViewModel viewModel, GMailLabelCleanup.Data.Models.Filters.Filter filter)
        {
            System.Diagnostics.Debug.Assert(viewModel != null);
            System.Diagnostics.Debug.Assert(filter != null);

            bool hasChanges = false;

            //Apply high level properties
            if (!string.Equals(filter.Description, viewModel.Description))
            {
                filter.Description = viewModel.Description;
                hasChanges = true;
            }

            foreach (var item in viewModel.ActionProperties.Concat(viewModel.CriteriaProperties).Where(o => o.IsCheckType))
            {
                item.Value = item.IsChecked ? "true" : "false";
            }

            /*
             * New properties:
             * ViewModel properties that are included and whose
             * name does not appear in the Filter.FilterProperties.
             * 
             * Updated properies:
             * ViewModel properties whose
             * name appears in the Filter.FilterProperties.
             */

            var existingPropertyNames = filter.FilterProperties
                .Select(o => o.Name)
                .ToHashSet();

            var allProperties = viewModel.CriteriaProperties
                .Concat(viewModel.ActionProperties)
                .ToArray();

            var newProperties = allProperties
                .Where(o => !existingPropertyNames.Contains(o.Name));
            var updatedProperties = allProperties
                .Where(o => existingPropertyNames.Contains(o.Name));

            var dateCreated = DateTime.UtcNow;

            foreach (var item in updatedProperties)
            {
                var matches = filter.FilterProperties
                   .Where(o => string.Equals(o.Name, item.Name) && (!string.Equals(o.Value, item.Value) || o.IsIncluded != item.IsIncluded))
                   .ToArray();

                foreach (var match in matches)
                {
                    match.IsIncluded = item.IsIncluded;
                    match.Value = item.Value;
                    hasChanges = true;
                }
            }
            foreach (var item in newProperties)
            {
                filter.FilterProperties.Add(new FilterProperty
                {
                    IsIncluded = item.IsIncluded,
                    Name = item.Name,
                    Value = item.Value,
                    DateCreatedUtc = dateCreated
                });
                hasChanges = true;
            }
            return hasChanges;
        }

        public async Task<ActionResult> Delete(int id)
        {
            var userId = GetUserId();
            var context = this.DbContext;

            var filter = await context.Filters
                .Where(o => o.FilterId == id && o.UserId == userId)
                .FirstOrDefaultAsync();
            if (filter == null)
                return HttpNotFound("Could not find filter.");

            context.Filters.Remove(filter);
            await context.SaveChangesAsync();

            await Task.Run(() => new TelemetryClient().TrackEvent("DeleteFilter"));

            return RedirectToAction("Index");
        }

        public ActionResult Import()
        {
            return View(new ImportViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Import(ImportViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            XDocument document;
            try
            {
                document = XDocument.Load(viewModel.UploadFile.InputStream);
            }
            catch (Exception)
            {
                ModelState.AddModelError("UploadFile", "Received invalid XML.");
                return View(viewModel);
            }

            var userId = GetUserId();

            await Task.Run(() => new TelemetryClient().TrackEvent("ImportFilters"));

            //Use MongoDb to temporarily store instead of the session.
            var mongoDbComponent = new MongoDbComponent();
            var collection = mongoDbComponent.MessageFilters;

            var filterComponent = new MessageFilterComponent();
            var id = await collection.InsertAsync(filterComponent.FromXml(document, userId));

            return RedirectToAction("ImportReview", new { id = id });
        }

        public async Task<ActionResult> ImportReview(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpNotFoundResult("Could not find import file.");

            var userId = GetUserId();

            var mongoDbComponent = new MongoDbComponent();
            var collection = mongoDbComponent.MessageFilters;

            //Pull down the file data.
            var messageFilter = await collection.FindOneByObjectIdUserIdAsync(id, userId);
            if (messageFilter == null)
                return new HttpNotFoundResult("Could not find import file.");

            var filterComponent = new MessageFilterComponent();

            var viewModel = new ReviewViewModel
            {
                MessageFilterId = messageFilter.Id.ToString(),
                Entries = messageFilter.Entries.Select(o => new ChooseMessageFilterEntryViewModel
                {
                    IsSelected = true,
                    EntryId = filterComponent.GetFilterIdPart(o.IdTag),
                    CriteriaProperties = filterComponent.GetCriteriaProperties(o.Properties).ToList(),
                    ActionProperties = filterComponent.GetActionProperties(o.Properties).ToList()
                }).ToList(),
                SelectAll = true
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ImportReview(string postAction, ReviewViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            if (!string.Equals(postAction, "trash", StringComparison.OrdinalIgnoreCase) && !string.Equals(postAction, "try another file", StringComparison.OrdinalIgnoreCase))
            {
                return await ImportContinue(viewModel);
            }
            else
            {
                return await ImportTrash(viewModel);
            }
        }
        private async Task<ActionResult> ImportContinue(ReviewViewModel viewModel)
        {
            System.Diagnostics.Debug.Assert(viewModel != null);

            var userId = GetUserId();

            var mongoDbComponent = new MongoDbComponent();
            var collection = mongoDbComponent.MessageFilters;

            //Pull down the file data.
            var messageFilter = await collection.FindOneByObjectIdUserIdAsync(viewModel.MessageFilterId, userId);
            if (messageFilter == null)
                return new HttpNotFoundResult("Could not find import file.");

            //Throw the selected entry ids into a hashset.
            var selectedEntries = viewModel.Entries
                .Where(o => o.IsSelected)
                .Select(o => o.EntryId)
                .ToHashSet();

            var context = this.DbContext;
            bool hasAdded = false;

            var dateCreatedUtc = DateTime.UtcNow;

            //Add it to the SQL server collection.
            var filterComponent = new MessageFilterComponent();
            foreach (var item in messageFilter.Entries)
            {
                var itemId = filterComponent.GetFilterIdPart(item.IdTag);
                if (!selectedEntries.Contains(itemId))
                    continue;

                context.Filters.Add(new GMailLabelCleanup.Data.Models.Filters.Filter
                {
                    UserId = userId,
                    ImportId = itemId,
                    Description = filterComponent.CreateDescription(item),
                    DateCreatedUtc = dateCreatedUtc,
                    FilterProperties = item.Properties.Select(o => new FilterProperty
                    {
                        IsIncluded = true,
                        Name = o.Name,
                        Value = o.Value,
                        DateCreatedUtc = dateCreatedUtc
                    }).ToList()
                });
                hasAdded = true;
            }

            if (hasAdded)
            {
                await context.SaveChangesAsync();
            }

            //Remove it from the MongoDb storage.
            await collection.RemoveAsync(messageFilter);

            return RedirectToAction("Index");
        }
        private async Task<ActionResult> ImportTrash(ReviewViewModel viewModel)
        {
            System.Diagnostics.Debug.Assert(viewModel != null);

            var userId = GetUserId();

            var mongoDbComponent = new MongoDbComponent();
            var collection = mongoDbComponent.MessageFilters;

            //Pull down the file data.
            var messageFilter = await collection.FindOneByObjectIdUserIdAsync(viewModel.MessageFilterId, userId);
            if (messageFilter == null)
                return new HttpNotFoundResult("Could not find import file.");

            //Remove it from the MongoDb storage.
            await collection.RemoveAsync(messageFilter);

            return RedirectToAction("Import");
        }

        public async Task<ActionResult> DeleteAllConfirm()
        {
            var userId = GetUserId();
            var context = this.DbContext;

            var count = await context.Filters
                .CountAsync(o => o.UserId == userId);

            var viewModel = new DeleteAllConfirmViewModel()
            {
                Count = count
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAllConfirm(string deleteAll, DeleteAllConfirmViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            if (string.Equals(deleteAll, "yes", StringComparison.OrdinalIgnoreCase))
            {
                var userId = GetUserId();
                var context = this.DbContext;

                var filters = await context.Filters
                    .Include(o => o.FilterProperties)
                    .Where(o => o.UserId == userId)
                    .ToArrayAsync();

                bool hasChanged = false;
                foreach (var item in filters)
                {
                    context.Filters.Remove(item);
                    hasChanged = true;
                }

                if (hasChanged)
                {
                    await context.SaveChangesAsync();

                    await Task.Run(() => new TelemetryClient().TrackEvent("DeleteAllFilter"));
                }
            }

            return RedirectToAction("Index");
        }

    }
}