using KPISearch.Models;
using Reachmail;
using Reachmail.Lists.Filtered.Post;
using Reachmail.Lists.Post;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;

namespace KPISearch
{
    public class UploadLists
    {
        private Dsl reachmail = Api.Create(ConfigurationManager.AppSettings["KPSA"]);
        private Reachmail.Administration.Users.Current.Get.Response.User currentUser = null;
        private List<EmailList> emailLists;
        private List<string> distinctLists;
        private List<ListDetail> lists;
        private List<FilesWithList> filesWithList;
        private string folderPath = "C:\\temp\\";

        public void UploadKPSARecords(string filePath)
        {
            try
            {
                reachmail = Reachmail.Api.Create(ConfigurationManager.AppSettings["KPSA"]);
                currentUser = reachmail.Administration.Users.Current.Get();
                string fileExtension = Path.GetExtension(filePath);
                emailLists = new List<EmailList>();
                emailLists = KPSADatatableToList(HelperMethods.ConvertExcelToDataTable(filePath, true));      
                distinctLists = emailLists.Select(d => d.ListName).Distinct().ToList();
                UploadFilesForLists();
                CreateNewLists();
                MergeListsWithFiles();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void CreateNewLists()
        {
            try
            {
                lists = new List<ListDetail>();
                foreach (var list in distinctLists)
                {
                    ListDetail listDetail = new Models.ListDetail();
                    var requestFilter = new ListFilter
                    {
                        Name = list,
                    };
                    var resultFilter = reachmail.Lists.Filtered.Post(requestFilter, currentUser.AccountId).FirstOrDefault();
                    if (resultFilter == null)
                    {
                        var request = new ListProperties
                        {
                            Name = list,
                            Type = ListType.Recipient
                        };

                        var result = reachmail.Lists.Post(request, currentUser.AccountId);
                        listDetail.Id = result.Id;
                        listDetail.ListName = list;
                        listDetail.ListStatus = "Newly Created";
                    }
                    else
                    {
                        listDetail.Id = resultFilter.Id;
                        listDetail.ListName = resultFilter.Name;
                        listDetail.ListStatus = "Updated";
                    }
                    lists.Add(listDetail);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void UploadFilesForLists()
        {
            try
            {
                filesWithList = new List<FilesWithList>();
                foreach (var list in distinctLists)
                {
                    int emailsCount = 0;
                    string listName = string.Concat(list, DateTime.Now.ToString("yyyyMMddHHmmssfff"), ".csv");
                    string filePath = String.Format(folderPath + listName);
                    using (CsvFileWriter writer = new CsvFileWriter(filePath))
                    {
                        var emails = emailLists.Where(d => d.ListName == list).Select(d => d.Email);
                        foreach (var email in emails)
                        {
                            CsvRow row = new CsvRow();
                            row.Add(email);
                            writer.WriteRow(row);
                        }
                        emailsCount = emails.Count();
                    }
                    var request = File.OpenRead(filePath);
                    var mimeType = "text/csv";
                    var result = reachmail.Data.Post(request, mimeType);
                    FilesWithList fileWithCampaign = new FilesWithList()
                    {
                        ListName = list,
                        Id = result.Id,
                        Records = emailsCount
                    };
                    filesWithList.Add(fileWithCampaign);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void MergeListsWithFiles()
        {
            try
            {
                foreach (var list in lists)
                {
                    var file = filesWithList.FirstOrDefault(d => d.ListName.ToLower() == list.ListName.ToLower());
                    if (file != null)
                    {
                        var request = new Reachmail.Lists.Import.Post.Parameters
                        {
                            DataId = file.Id,
                            FieldMappings = new List<Reachmail.Lists.Import.Post.FieldMapping>
    {
        new Reachmail.Lists.Import.Post.FieldMapping
        {
            DestinationFieldName = "Email",
            SourceFieldPosition = 1,
        }
    },
                            ImportOptions = new Reachmail.Lists.Import.Post.ImportOptions
                            {
                                SkipRecordsWithErrors = true,
                                AllowStringTruncation = true,
                                AbortImportOnError = false,
                            }
                        };
                        var result = reachmail.Lists.Import.Post(request, currentUser.AccountId, Guid.Parse(list.Id.ToString()));
                        list.Records = file.Records;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<EmailList> KPSADatatableToList(DataTable dt)
        {
            try
            {
                List<EmailList> emailList = new List<EmailList>();
                return emailList = (from DataRow dr in dt.Rows
                                    select new EmailList()
                                    {
                                        Email = dr["Email"].ToString(),
                                        ListName = dr["List Name"].ToString().Trim(),
                                        Account = dr["Account"].ToString()
                                    }).Where(d => d.Account != "" && d.ListName != "" && d.Email != "").ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
