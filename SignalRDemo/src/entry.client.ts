tryHydrateReactComponents();

async function tryHydrateReactComponents() {
  // Initialise React if there are any React components.
  if (!document.querySelector('react-component')) {
    return;
  }

  const [React, ReactDOM, components] = await Promise.all([
    import('react'),
    import('react-dom'),
    import('./components'),
  ]);

  window.React = React.default;
  window.ReactDOM = ReactDOM.default;
  window.Components = components.default;

  if (
    "ReactJsAsyncInit" in window &&
    typeof window.ReactJsAsyncInit === 'function'
  ) {
    window.ReactJsAsyncInit();
  }
}