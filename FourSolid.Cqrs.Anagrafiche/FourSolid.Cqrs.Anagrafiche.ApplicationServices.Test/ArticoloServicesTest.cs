﻿using System;
using System.Threading.Tasks;
using Autofac;
using FourSolid.Cqrs.Anagrafiche.Domain.Rules.Resources;
using FourSolid.Cqrs.Anagrafiche.Shared.ApplicationServices;
using FourSolid.Cqrs.Anagrafiche.Shared.JsonModel;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.Services;
using FourSolid.Shared.ValueObjects;
using Xunit;

namespace FourSolid.Cqrs.Anagrafiche.ApplicationServices.Test
{
    public class ArticoloServicesTest
    {
        private readonly IContainer _container;

        private readonly AccountInfo _who = new AccountInfo(new AccountId(Guid.NewGuid().ToString()),
            new AccountName("Tester"), new AccountRole("T"));
        private readonly When _when = new When(DateTime.UtcNow);

        public ArticoloServicesTest()
        {
            var builder = AutofacBootstrapper.RegisterModules();
            this._container = builder.Build();
        }

        [Fact]
        public async Task Cannot_CreateArticolo_Without_ArticoloDescrizione()
        {
            var articoloOrchestrator = this._container.Resolve<IArticoloOrchestrator>();
            var articoloJson = new ArticoloJson
            {
                ArticoloId = string.Empty,
                ArticoloDescrizione = string.Empty,
                UnitaMisura = "NR",
                ScortaMinima = 1
            };

            Exception ex = await Assert.ThrowsAnyAsync<Exception>(() =>
                articoloOrchestrator.CreateArticoloAsync(articoloJson, this._who, this._when));

            Assert.Equal(DomainExceptions.ArticoloDescrizioneNullException, CommonServices.GetErrorMessage(ex));
        }

        [Fact]
        public async Task Get_Articoli()
        {
            var articoloOrchestrator = this._container.Resolve<IArticoloOrchestrator>();

            await articoloOrchestrator.GetArticoliAsync();
        }
    }
}
