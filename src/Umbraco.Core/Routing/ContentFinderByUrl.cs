using Microsoft.Extensions.Logging;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;

namespace Umbraco.Web.Routing
{
    /// <summary>
    /// Provides an implementation of <see cref="IContentFinder"/> that handles page nice URLs.
    /// </summary>
    /// <remarks>
    /// <para>Handles <c>/foo/bar</c> where <c>/foo/bar</c> is the nice URL of a document.</para>
    /// </remarks>
    public class ContentFinderByUrl : IContentFinder
    {
        private readonly ILogger<ContentFinderByUrl> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentFinderByUrl"/> class.
        /// </summary>
        public ContentFinderByUrl(ILogger<ContentFinderByUrl> logger, IUmbracoContextAccessor umbracoContextAccessor)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            UmbracoContextAccessor = umbracoContextAccessor ?? throw new System.ArgumentNullException(nameof(umbracoContextAccessor));
        }

        /// <summary>
        /// Gets the <see cref="IUmbracoContextAccessor"/>
        /// </summary>
        protected IUmbracoContextAccessor UmbracoContextAccessor { get; }

        /// <summary>
        /// Tries to find and assign an Umbraco document to a <c>PublishedRequest</c>.
        /// </summary>
        /// <param name="frequest">The <c>PublishedRequest</c>.</param>
        /// <returns>A value indicating whether an Umbraco document was found and assigned.</returns>
        public virtual bool TryFindContent(IPublishedRequestBuilder frequest)
        {
            IUmbracoContext umbCtx = UmbracoContextAccessor.UmbracoContext;
            if (umbCtx == null)
            {
                return false;
            }

            string route;
            if (frequest.Domain != null)
            {
                route = frequest.Domain.ContentId + DomainUtilities.PathRelativeToDomain(frequest.Domain.Uri, frequest.Uri.GetAbsolutePathDecoded());
            }
            else
            {
                route = frequest.Uri.GetAbsolutePathDecoded();
            }

            IPublishedContent node = FindContent(frequest, route);
            return node != null;
        }

        /// <summary>
        /// Tries to find an Umbraco document for a <c>PublishedRequest</c> and a route.
        /// </summary>
        /// <returns>The document node, or null.</returns>
        protected IPublishedContent FindContent(IPublishedRequestBuilder docreq, string route)
        {
            IUmbracoContext umbCtx = UmbracoContextAccessor.UmbracoContext;
            if (umbCtx == null)
            {
                return null;
            }

            if (docreq == null)
            {
                throw new System.ArgumentNullException(nameof(docreq));
            }

            _logger.LogDebug("Test route {Route}", route);

            IPublishedContent node = umbCtx.Content.GetByRoute(umbCtx.InPreviewMode, route, culture: docreq.Culture?.Name);
            if (node != null)
            {
                docreq.SetPublishedContent(node);
                _logger.LogDebug("Got content, id={NodeId}", node.Id);
            }
            else
            {
                _logger.LogDebug("No match.");
            }

            return node;
        }
    }
}
