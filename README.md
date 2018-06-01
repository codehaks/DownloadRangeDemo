# Serving files with Range Mode in ASP.NET Core 2.1

Usually you use **PhysicalFileResult** to send a file back to the user. In ASP.NET Core 2.1 there is a new overload for **PhysicalFileResult** constructor that let's you choose if you want to send file in Range mode.

Range mode enables the file to be downloaded in multiple parts, That way a download manager software can make multiple connections to your sever and download the file faster.

You could do this before, But you didn't have any option not to use it, Now there is a new Argument called **EnableRangeProccessing** for that.
