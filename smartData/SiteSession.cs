using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Globalization;
//
//using MvcBasic.Logic;


namespace MvcBasicSite.Models
{
    /// <summary>
    /// Defines the site session.
    /// </summary>
    /// <remarks>
    /// Used to cache only the needed data for the current user!
    /// </remarks>
    public class SiteSession
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the user role.
        /// </summary>
        //public UserRoles UserRole { get; set; }
        /// <summary>
        /// Gets or sets the current UI culture.
        /// </summary>
        /// <remarks>
        /// Values meaning: 0 = InvariantCulture (en-US), 1 = ro-RO, 2 = de-DE.
        /// </remarks>
        public static int CurrentUICulture
        {
            get
            {
                if (HttpContext.Current.Session["CurrentUICulture"] != null)
                {
                    if ((int)HttpContext.Current.Session["CurrentUICulture"] == 1)
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo("ro-RO");
                    else if ((int)HttpContext.Current.Session["CurrentUICulture"] == 2)
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
                    else
                        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                }
                if (Thread.CurrentThread.CurrentUICulture.Name == "ro-RO")
                    return 1;
                else if (Thread.CurrentThread.CurrentUICulture.Name == "de-DE")
                    return 2;
                else
                    return 0;
            }
            set
            {
                //
                // Set the thread's CurrentUICulture.
                //
                if (value == 1)
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("ro-RO");
                else if (value == 2)
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
                else
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                //
                // Set the thread's CurrentCulture the same as CurrentUICulture.
                //
                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            }
        }

        /// <summary>
        /// Initializes a new instance of the SiteSession class.
        /// </summary>
        /// <param name="db">The data context.</param>
        /// <param name="user">The current user.</param>
        //public SiteSession(MvcBasicSiteEntities db, User user)
        //{
        //    this.UserID = user.ID;
        //    this.Username = user.Username;
        //    this.UserRole = user.UserRole;
        //    //
        //    // TO DO: Cache other user settings!
        //    //
        //}
    }
}