export default defineNuxtRouteMiddleware((to, from) => {
  const token = localStorage.getItem('token');
  if (!token && to.path !== '/' && to.path !== '/register') {
    return navigateTo('/');
  }
});
