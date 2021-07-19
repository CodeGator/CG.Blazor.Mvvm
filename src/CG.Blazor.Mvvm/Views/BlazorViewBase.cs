using CG.Blazor.Mvvm.ViewModels;
using CG.Mvvm.ViewModels;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace CG.Blazor.Mvvm.Views
{
    /// <summary>
    /// This class is a base implementation of a Blazor view.
    /// </summary>
    /// <typeparam name="T">The type of associated view-model.</typeparam>
    public abstract class BlazorViewBase<T> : ComponentBase, IDisposable
        where T : class, IViewModel
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains an associated view-model.
        /// </summary>
        [Inject]
        protected T ViewModel { get; set; }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method performs application-defined tasks associated with 
        /// freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
#pragma warning disable CS1998 
            ViewModel.PropertyChanged -= async (sender, e) => { };
#pragma warning restore CS1998 
        }

        #endregion

        // *******************************************************************
        // Protected methods.
        // *******************************************************************

        #region Protected methods

        /// <summary>
        /// This method is invoked when the component is ready to start, having 
        /// received its initial parameters from its parent in the render tree.
        /// </summary>
        /// <returns>A task to perform the operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            // Do we have a view-model instance?
            if (null != ViewModel)
            {
                // Wire up a handler for any view-model property changes.
                ViewModel.PropertyChanged += async (sender, e) =>
                {
                    // Tell Blazor whenever something changes.
                    await InvokeAsync(() => StateHasChanged());
                };
            }

            // Is the view-model a Blazor view-model?
            if (ViewModel is BlazorViewModelBase)
            {
                // Initialize the view-model.
                await (ViewModel as BlazorViewModelBase).OnInitializedAsync()
                    .ConfigureAwait(false);
            }

            // Give the base class a chance.
            await base.OnInitializedAsync()
                .ConfigureAwait(false);
        }

        #endregion
    }
}
