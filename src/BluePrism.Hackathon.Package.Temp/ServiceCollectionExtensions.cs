//using System;
//using System.Collections.Generic;
//using System.Text;
//using BluePrism.Hackathon.Package.Temp.Contracts;
//using Microsoft.Extensions.DependencyInjection;

//namespace BluePrism.Hackathon.Package.Temp
//{
//    public static class ServiceCollectionExtensions
//    {
//        //public static IServiceCollection AddWordLadder(this IServiceCollection services, Func<WordLadderOptions> options)
//        //{
//        //    services.AddOptions();
//        //    services.AddSingleton<WordLadderOptions>(options.Invoke());
//        //    services.AddSingleton<IWordLadder, WordLadder>();
//        //    return services;
//        //}

//        /// <summary>
//        /// Adds services required for application localization.
//        /// </summary>
//        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
//        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
//        public static IServiceCollection AddWordLadder(this IServiceCollection services)
//        {
//            if (services == null)
//            {
//                throw new ArgumentNullException(nameof(services));
//            }

//            services.AddOptions();

//            AddWordLadderServices(services);

//            return services;
//        }

//        /// <summary>
//        /// Adds services required for application localization.
//        /// </summary>
//        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
//        /// <param name="setupAction">
//        /// An <see cref="Action{LocalizationOptions}"/> to configure the <see cref="LocalizationOptions"/>.
//        /// </param>
//        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
//        public static IServiceCollection AddWordLadder(
//            this IServiceCollection services,
//            Action<WordLadderOptions> setupAction)
//        {
//            if (services == null)
//            {
//                throw new ArgumentNullException(nameof(services));
//            }

//            if (setupAction == null)
//            {
//                throw new ArgumentNullException(nameof(setupAction));
//            }

//            AddWordLadderServices(services, setupAction);

//            return services;
//        }

//        // To enable unit testing
//        internal static void AddWordLadderServices(IServiceCollection services)
//        {
//            //services.TryAddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();
//            //services.TryAddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
//        }

//        internal static void AddWordLadderServices(
//            IServiceCollection services,
//            Action<WordLadderOptions> setupAction)
//        {
//            AddWordLadderServices(services);
//            services.Configure(setupAction);
//        }
//    }
//}
