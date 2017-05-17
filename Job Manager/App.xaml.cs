using Job_Manager.UserIdentity;
using Job_Manager.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Job_Manager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// Verifying JIT checkin and the Username
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            //Create a custom principal with an anonymous identity at startup
            
            CustomPrincipal customPrincipal = new CustomPrincipal();
            AppDomain.CurrentDomain.SetThreadPrincipal(customPrincipal);

            base.OnStartup(e);

            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            
            SplashScreen splash = new SplashScreen();
            splash.Show();            
            Stopwatch timer = new Stopwatch();
            timer.Start();    
                    
            base.OnStartup(e);
            AuthenticationViewModel viewModel = new AuthenticationViewModel(new AuthenticationService());
            var loginWindow = new LoginWindow(viewModel) { };
            viewModel.OnRequestClose += (s, evt) => loginWindow.Close();

            timer.Stop();

            int MINIMUM_SPLASH_TIME = 3000;
            int remainingTimeToShowSplash = MINIMUM_SPLASH_TIME - (int)timer.ElapsedMilliseconds;
            if (remainingTimeToShowSplash > 0)
                Thread.Sleep(remainingTimeToShowSplash);

            splash.Close();
            

            //Show the login view and Dependency Injection         
            loginWindow.ShowDialog();
            

            //AttributeViewModel objAttributeViewModel = new AttributeViewModel();
            //var attributeView = new Attribute(objAttributeViewModel);
            //attributeView.ShowDialog();

           // Attribute_New objAttribute_New = new Attribute_New();
           // objAttribute_New.ShowDialog();

            //AttributeMainScreen objAttributeMainScreen = new AttributeMainScreen();
            //objAttributeMainScreen.ShowDialog();

           // MaterialScreen objMaterialMainScreen = new MaterialScreen();
           // objMaterialMainScreen.ShowDialog();

         //   MaterialMainScreen objMaterialMainScreen = new MaterialMainScreen();
        //    objMaterialMainScreen.ShowDialog();

            //VendorMainScreen objVendor = new VendorMainScreen();
            //objVendor.ShowDialog();

            //VendorScreen objVendorScreen = new VendorScreen();
            //objVendorScreen.ShowDialog();

            //UserMainScreen objUserMainScreen = new UserMainScreen();
            //objUserMainScreen.ShowDialog();
            //UserMainScreen objUserMainScreen = new UserMainScreen();
            //objUserMainScreen.ShowDialog();
        }
    }
}
