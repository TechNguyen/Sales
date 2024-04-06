using Microsoft.AspNetCore.Http;
using Sale.Service.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Service.Common
{
	public static class FileExtension
	{
		public static IDictionary<string,string>? GetDateTimeFile(string FileName)
		{
			int dotIndex = FileName.LastIndexOf('.');
            if (dotIndex >= 0)
            {
				var fileNamePath = FileName.Substring(0, dotIndex);
				var extension = FileName.Substring(dotIndex+1);
				var fullnameconvert = $"{fileNamePath}-{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}.{extension}"; 
				IDictionary<string,string> newRs = new Dictionary<string, string>();
				newRs.Add(new KeyValuePair<string, string>(FileConstant.FILENAME, fileNamePath));
				newRs.Add(new KeyValuePair<string, string>(FileConstant.FILEXTEMSION, extension));
				newRs.Add(new KeyValuePair<string, string>(FileConstant.FILENAMECONVERT, fullnameconvert));
				return newRs;
			} else
			{
				return null;
			}
        }


		public static bool UploadFile(IFormFile file)
		{
			try
			{
				if(file.Length > 0 && file != null)
				{
					var nameFile = file.FileName;
					var keyvalue = GetDateTimeFile(nameFile);
					var pathFile = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", keyvalue[FileConstant.FILENAMECONVERT]);
					if (!File.Exists(pathFile))
					{
						using(var stream = new FileStream(pathFile, FileMode.Create))
						{
							file.CopyTo(stream);
							return true;
						}
					}
					else {
						return false;
					}
				}
				return false;
			}
			catch (Exception e)
			{
				return false; 
			}
		}
	}
}
