﻿using JNogueira.NotifiqueMe;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace JNogueira.Discord.Webhook.Client
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DiscordMessageEmbed : Notificavel
    {
        /// <summary>
        /// Color code of the embed. You have to use Decimal numeral system, not Hexadecimal. You can use SpyColor (https://www.spycolor.com) for that. It has a decimal number converter.
        /// </summary>
        [JsonProperty("color")]
        public int? Color { get; private set; }

        /// <summary>
        ///  Embed author object
        /// </summary>
        [JsonProperty("author")]
        public DiscordMessageEmbedAuthor Author { get; private set; }

        /// <summary>
        /// Title of embed
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; private set; }

        /// <summary>
        /// Url of embed. If title was used, it becomes hyperlink
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; private set; }

        /// <summary>
        /// Description text
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; private set; }

        /// <summary>
        /// Array of embed field objects
        /// </summary>
        [JsonProperty("fields")]
        public DiscordMessageEmbedField[] Fields { get; private set; }

        /// <summary>
        /// Embed thumbnail object
        /// </summary>
        [JsonProperty("thumbnail")]
        public DiscordMessageEmbedThumbnail Thumbnail { get; private set; }

        /// <summary>
        /// Embed image object
        /// </summary>
        [JsonProperty("image")]
        public DiscordMessageEmbedImage Image { get; private set; }

        /// <summary>
        /// Embed footer object
        /// </summary>
        [JsonProperty("footer")]
        public DiscordMessageEmbedFooter Footer { get; private set; }

        [JsonConstructor]
        private DiscordMessageEmbed()
        {

        }

        public DiscordMessageEmbed(
            string title = null,
            int? color = null,
            DiscordMessageEmbedAuthor author = null,
            string url = null,
            string description = null,
            IEnumerable<DiscordMessageEmbedField> fields = null,
            DiscordMessageEmbedThumbnail thumbnail = null,
            DiscordMessageEmbedImage image = null,
            DiscordMessageEmbedFooter footer = null)
        {
            this.Color       = color;
            this.Author      = author;
            this.Title       = title;
            this.Url         = url?.ToLower();
            this.Description = description;
            this.Fields      = fields?.ToArray();
            this.Thumbnail   = thumbnail;
            this.Image       = image;
            this.Footer      = footer;

            Validate();
        }

        internal void Validate()
        {
            this
                .NotificarSeVerdadeiro(!string.IsNullOrEmpty(this.Title) && this.Title?.Length > 256, $"The embed \"title\" length limit is 256 characters (actual lenght is {this.Title?.Length}).")
                .NotificarSeVerdadeiro(!string.IsNullOrEmpty(this.Description) && this.Description?.Length > 2048, $"The embed \"description\" length limit is 2048 characters (actual lenght is {this.Description?.Length}).");

            if (this.Fields?.Any() == true)
            {
                this.NotificarSeVerdadeiro(this.Fields.Length > 25, $"The embed \"fields\" collection size limit is 25 objects. (actual size is {this.Fields.Length})");

                this.Fields.ToList().ForEach(x => this.AdicionarNotificacoes(x));
            }

            this.AdicionarNotificacoes(this.Author);
            this.AdicionarNotificacoes(this.Thumbnail);
            this.AdicionarNotificacoes(this.Image);
            this.AdicionarNotificacoes(this.Footer);
        }
    }

    /// <summary>
    /// Embed author object
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class DiscordMessageEmbedAuthor : Notificavel
    {
        /// <summary>
        /// Name of author
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// Url of author. If name was used, it becomes a hyperlink
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; private set; }

        /// <summary>
        /// Url of author icon
        /// </summary>
        [JsonProperty("icon_url")]
        public string IconUrl { get; private set; }

        [JsonConstructor]
        private DiscordMessageEmbedAuthor()
        {

        }

        public DiscordMessageEmbedAuthor(string name, string url = null, string iconUrl = null)
        {
            this.Name    = name;
            this.Url     = url;
            this.IconUrl = iconUrl;

            Validate();
        }

        internal void Validate()
        {
            this
                .NotificarSeNuloOuVazio(this.Name, "The embed author \"name\" cannot be null or empty.")
                .NotificarSeVerdadeiro(!string.IsNullOrEmpty(this.Name) && this.Name.Length > 256, $"The embed author \"name\" length limit is 256 characters (actual lenght is {this.Name.Length}))");
        }
    }

    /// <summary>
    /// Embed field objects
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class DiscordMessageEmbedField : Notificavel
    {
        /// <summary>
        /// The name of the field
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// The value of the field
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; private set; }

        /// <summary>
        /// If true, fields will be displayed in the same line, but there can only be 3 max in the same line or 2 max if you used thumbnail
        /// </summary>
        [JsonProperty("inline")]
        public bool InLine { get; private set; }

        [JsonConstructor]
        private DiscordMessageEmbedField()
        {

        }

        public DiscordMessageEmbedField(string name, string value = null, bool inLine = false)
        {
            Name   = name;
            Value  = value;
            InLine = inLine;

            Validate();
        }

        public void Validate()
        {
            this
                .NotificarSeNuloOuVazio(this.Name, "The embed field \"name\" cannot be null or empty.")
                .NotificarSeVerdadeiro(!string.IsNullOrEmpty(this.Name) && this.Name.Length > 256, $"The embed field \"name\" length limit is 256 characters (actual lenght is {this.Name?.Length}))")
                .NotificarSeVerdadeiro(!string.IsNullOrEmpty(this.Value) && this.Value.Length > 1024, $"The embed field \"value\" length limit is 1024 characters (actual lenght is {this.Value?.Length}))");
        }
    }

    /// <summary>
    /// Embed thumbnail object
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class DiscordMessageEmbedThumbnail : Notificavel
    {
        /// <summary>
        /// url of thumbnail
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; private set; }

        [JsonConstructor]
        private DiscordMessageEmbedThumbnail()
        {

        }

        public DiscordMessageEmbedThumbnail(string url)
        {
            this.Url = url;

            Validate();
        }

        internal void Validate()
        {
            this.NotificarSeNuloOuVazio(this.Url, "The embed thumbnail \"url\" cannot be null or empty.");
        }
    }

    /// <summary>
    /// Embed image object
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class DiscordMessageEmbedImage : Notificavel
    {
        /// <summary>
        /// url of image
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; private set; }

        [JsonConstructor]
        private DiscordMessageEmbedImage()
        {

        }

        public DiscordMessageEmbedImage(string url)
        {
            this.Url = url;

            Validate();
        }

        internal void Validate()
        {
            this.NotificarSeNuloOuVazio(this.Url, "The embed image \"url\" cannot be null or empty.");
        }
    }

    /// <summary>
    /// Embed footer object
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class DiscordMessageEmbedFooter : Notificavel
    {
        /// <summary>
        /// Footer text, doesn't support Markdown
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; private set; }

        /// <summary>
        /// Url of footer icon
        /// </summary>
        [JsonProperty("icon_url")]
        public string IconUrl { get; private set; }

        [JsonConstructor]
        private DiscordMessageEmbedFooter()
        {

        }

        public DiscordMessageEmbedFooter(string text, string iconUrl = null)
        {
            this.Text    = text;
            this.IconUrl = iconUrl;

            Validate();
        }

        internal void Validate()
        {
            this
                .NotificarSeNuloOuVazio(this.Text, "The embed footer \"text\" cannot be null or empty.")
                .NotificarSeVerdadeiro(!string.IsNullOrEmpty(this.Text) && this.Text.Length > 2048, $"The embed footer \"text\" length limit is 2048 characters (actual lenght is {this.Text.Length}))");
        }
    }
}
