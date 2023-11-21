﻿using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;
using System.ComponentModel;

namespace StudentManagementSystem.Controllers
{
    public class StorageController : Controller
    {

        private ApplicationUserManager _userManager;

        public StorageController(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var student = _userManager.GetUserAsync(User).Result;


            return View();
        }

        public IActionResult FileUpload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Get the uploaded file
            var file = Request.Form.Files.GetFile("file");

            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file", "Please select a file to upload");
                return View("FileUpload");
            }

            // Get the folder path from the user's FolderPath property
            string folderPath = user.FolderPath;

            // Ensure the folder exists, you might want additional error handling here
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Combine the folder path with the file name to get the full path
            var filePath = Path.Combine(folderPath, file.FileName);

            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return RedirectToAction("Index", "Home"); // Redirect to wherever you want after successful upload
        }
        public IActionResult ShowFiles()
        {
            var user = _userManager.GetUserAsync(User).Result;

            string directoryPath = user.FolderPath;

            List<Product> productList = new List<Product>();

            try
            {
                // Check if the directory exists
                if (Directory.Exists(directoryPath))
                {
                    // Get all files in the directory
                    string[] fileEntries = Directory.GetFiles(directoryPath);

                    foreach (string filePath in fileEntries)
                    {
                        // Get file information
                        FileInfo fileInfo = new FileInfo(filePath);

                        // Create a Product object with file details
                        Product product = new Product
                        {
                            FileName = Path.GetFileName(filePath),

                            FileSize = (fileInfo.Length / (1024.0 * 1024.0)).ToString("0.##") + " MB",

                            FilePath = fileInfo.DirectoryName,

                            UserId=user.Email

                        };

                        // Add the product to the list
                        productList.Add(product);
                    }
                }
                else
                {
                    Console.WriteLine("Directory does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
           
            return View(productList);
        }
        public IActionResult DeleteFile([FromBody] string filePath)
        {
            try

            {
                var user = _userManager.GetUserAsync(User).Result;
                string path=Path.Combine(user.FolderPath, filePath);
                

             
                // Ensure the filePath is not empty and is a valid path
             /*   if (string.IsNullOrEmpty(path) || !Path.IsPathRooted(path))
                {
                    return Json(new { success = false, message = "Invalid file path." });
                }
*/
                // Check if the file exists
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return RedirectToAction("ShowFiles");
                }
                else
                {
                    return Json(new { success = false, message = "File not found." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error deleting file: {ex.Message}" });
            }
        }


    }
}
