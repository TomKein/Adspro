import { createRouter, createWebHistory } from 'vue-router';
import Login from '@/views/Login.vue';
import Welcome from '@/views/Welcome.vue';
import Users from '@/views/Users.vue';

const routes = [
    {
        path: '/',
        redirect: '/welcome',
    },
    {
        path: '/login',
        name: 'Login',
        component: Login,
    },
    {
        path: '/welcome',
        name: 'Welcome',
        component: Welcome,
        meta: { requiresAuth: true },
    },
    {
        path: '/users',
        name: 'Users',
        component: Users,
    },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

router.beforeEach((to, from, next) => {
    const isAuthenticated = localStorage.getItem('token');

    if (to.matched.some((record) => record.meta.requiresAuth)) {
        if (!isAuthenticated) {
            next('/login'); 
        } else {
            next();
        }
    } else {
        next();
    }
});

export default router;