# Serving files with Range Mode in ASP.NET Core 2.1

Usually you use **PhysicalFileResult** to send a file back to the user. In ASP.NET Core 2.1 there is a new overload for **PhysicalFileResult** constructor that let's you send file in Range mode.

Range mode enables the file to be downloaded it multiple parts, That way a download manager software can make multiple connections to your sever and download the file faster.

You could do this before, but you had to write the code on your own, Now this is much easier to implement !
