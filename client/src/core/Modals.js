import { store } from 'react-notifications-component';

export const errorModal = (err) => {
    store.addNotification({
        title: 'Error',
        message: err,
        type: 'danger',
        insert: 'top',
        container: 'top-right',
        animationIn: ["animate__animated animate__fadeIn"],
        animationOut: ["animate__animated animate__fadeOut"],
        dismiss: {
            duration: 4000,
            onScreen: true,
            pauseOnHover: true,
            showIcon: true
        }
    })
}

export const warningModal = (message) => {
    store.addNotification({
        title: 'Warning',
        message: message ?? 'Something went wrong',
        type: 'warning',
        insert: 'top',
        container: 'top-right',
        animationIn: ["animate__animated animate__fadeIn"],
        animationOut: ["animate__animated animate__fadeOut"],
        dismiss: {
            duration: 5000,
            onScreen: true,
            pauseOnHover: true,
            showIcon: true
        }
    })
}

export const successModal = (message) => {
    store.addNotification({
        title: 'Success!',
        message: message ?? '',
        type: 'success',
        insert: 'top',
        container: 'top-right',
        animationIn: ["animate__animated animate__fadeIn"],
        animationOut: ["animate__animated animate__fadeOut"],
        dismiss: {
            duration: 5000,
            onScreen: true,
            pauseOnHover: true,
            showIcon: true
        }
    })
}

export const infoModal = (message) => {
    store.addNotification({
        title: 'Info',
        message: message ?? '',
        type: 'info',
        insert: 'top',
        container: 'top-right',
        animationIn: ["animate__animated animate__fadeIn"],
        animationOut: ["animate__animated animate__fadeOut"],
        dismiss: {
            duration: 5000,
            onScreen: true,
            pauseOnHover: true
        }
    })
}