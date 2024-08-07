﻿using System.Text;
using System.Text.Json;
using Client.Components.Tailwind;
using Client.Models;
using Client.Shared;
using Client.Store.Base;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace Client.Components;

partial class LoginModal
{
    private Modal _modalRef;

    private UserLoginRequest _userLogin = new();

    /*
     private Timer _timer = default!;
    private string _toast = "invisible";


    protected override void OnInitialized() {
        _timer = new Timer(2000) { AutoReset = false };
    }*/
    [Inject] private IDispatcher Dispatcher { get; set; }

    private async Task Login()
    {
        using StringContent jsonContent = new(JsonSerializer.Serialize(_userLogin), Encoding.UTF8, "application/json");
        try
        {
            var response = await _httpClient.PostAsync($"{EnvUrls.User}login", jsonContent);
            var token = await response.Content.ReadAsStringAsync();
            if (token != null && token != "null")
            {
                await _localStorageService.SetItemAsStringAsync("token", token);
                await _authStateProvider.GetAuthenticationStateAsync();
                /*TODO(Not able to get the snackbar working) TriggerSnackbar();*/
                _toast.ShowSuccess("Logged in Successfully");
                Dispatcher.Dispatch(new FetchBaseDataAction());
            }
            else
            {
                _toast.ShowError("Couldn't Log in");
            }
        }
        catch (Exception ex)
        {
            _userLogin = new UserLoginRequest();
            _toast.ShowError($"{ex.Message}");
            await _modalRef.CloseModal();
        }
    }

    private async Task Cancel()
    {
        _userLogin = new UserLoginRequest();
        /*_toast.ShowInfo("Closed Login Modal");*/
        await _modalRef.CloseModal();
    }

    /*void TriggerSnackbar() {
        _timer.Start();
        _toast = "";
        _timer.Elapsed += (sender, args) => { _toast = "invisible";
            StateHasChanged();
            _timer.Dispose();
        };
    }*/

    /*void IDisposable.Dispose()
        => _timer.Dispose();*/
}