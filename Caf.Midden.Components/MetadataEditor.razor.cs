﻿using Caf.Midden.Core.Models.v0_1_0alpha4;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;

namespace Caf.Midden.Components
{
    public partial class MetadataEditor : ComponentBase
    {
        [Parameter]
        public Metadata Metadata { get; set; }

        [Parameter]
        public EventCallback<Metadata> MetadataChanged { get; set; }

        private string LastUpdated { get; set; } =
            DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

        private void SetMetadata(Metadata m)
        {
            if(this.Metadata != m)
            {
                this.Metadata = m;
                LastUpdated = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                MetadataChanged.InvokeAsync(this.Metadata);
            }
        }

        private EditContext EditContext { get; set; }
        
        public void LoadMetadataFile()
        {
            // Mock for now
            var now = DateTime.UtcNow;

            Metadata metadata = new Metadata()
            {
                CreationDate = now,
                ModifiedDate = now,
                Dataset = new Dataset()
                {
                    Zone = "Raw",
                    Name = "Test",
                    Contacts = new List<Person>()
                    {
                        new Person()
                        {
                            Name = "Test",
                            Email = "Test@test.com",
                            Role = "Tool"
                        }
                    }
                }
            };

            SetMetadata(metadata);  
        }

        protected override void OnInitialized()
        {
            this.EditContext = new EditContext(this.Metadata);
            this.EditContext.OnFieldChanged +=
                EditContext_OnFieldChange;
        }

        private void EditContext_OnFieldChange(
            object sender, 
            FieldChangedEventArgs e)
        {
            MetadataChanged.InvokeAsync(this.Metadata);
            LastUpdated = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }
    }
}
