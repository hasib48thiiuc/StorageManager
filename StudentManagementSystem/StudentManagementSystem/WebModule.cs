﻿using Autofac;
using StudentManagementSystem.Models;

namespace StudentManagementSystem
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RegisterModel>().AsSelf();
           builder.RegisterType<LoginModel>().AsSelf();

            base.Load(builder);
        }
    }
}
