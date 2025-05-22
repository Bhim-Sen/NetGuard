using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using NetGuardUI.Data.Extension;
using NetGuardUI.Infrastructure;
using System.Security.Claims;

namespace DealLayout.Shared
{
	public partial class NavMenu : ComponentBase
	{

		string username { get; set; }
		[CascadingParameter] private Task<AuthenticationState> authStateTask { get; set; }


		private ClaimsPrincipal User;

		[Inject] IJSRuntime JSRuntime { get; set; } = default!;
		[Inject] EncryptionService encryptionService { get; set; } = null!;
		[Inject] WebsiteAuthenticator customAuth { get; set; } = default!;
 
		[Inject] NavigationManager NavigationManager { get; set; } = default!;
		[Inject] protected ToastService ToastService { get; set; } = default!;


		private bool isAdminOrSuperAdmin;
		private bool isAdminOrSuperAdminOrManager;
		private bool isManager;
		private bool isNotCustomer;
		private bool isVendorAndManager;
		private string Avatar;
		private bool NoResultFound = false;
		private string searchQuery = string.Empty;
		private bool SearchLoader = false;
		private Timer? _debounceTimer; private const int DebounceDelay = 1500;
		private bool showDropdown = false;


		public int totalOrderItems;


		protected override async Task OnInitializedAsync()
		{
			var customState = await customAuth.GetAuthenticationStateAsync();
			User = customState.User;
			if (User != null && User.Identity.IsAuthenticated)
			{

			}
		}

 

		protected override async Task OnAfterRenderAsync(bool first)
		{
			if (first)
			{
				await JSRuntime.InvokeVoidAsync("navlist");
				await JSRuntime.InvokeVoidAsync("addClickOutsideListener", DotNetObjectReference.Create(this));
			}
		}

		

		private void ShowDropdown()
		{
			showDropdown = true;
			StateHasChanged();
		}

		[JSInvokable]
		public void CloseDropdown()
		{
			showDropdown = false;
			StateHasChanged();
		}



		private async Task NavigateToSkippedBanners()
		{
			NavigationManager.NavigateTo($"/");
			await Task.Delay(100);
			NavigationManager.NavigateTo($"#OfSkippedBanner", true);

		}
		private string GetHeaderText()
		{
			Uri currentUri = new Uri(NavigationManager.Uri);
			if (currentUri.Host.Contains("localhost"))
			{
				return "(Local)";
			}
			else
			{
				return "(Live)";
			}
		}
	}
}
