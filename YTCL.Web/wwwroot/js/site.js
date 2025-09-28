// Intersection Observer for lazy loading
window.setupIntersectionObserver = (element, dotnetHelper) => {
    if (!element) return;
    
    const options = {
        root: null,
        rootMargin: '0px',
        threshold: 0.1
    };
    
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                dotnetHelper.invokeMethodAsync('OnIntersection');
            }
        });
    }, options);
    
    observer.observe(element);
    
    // Return a function to clean up the observer
    return () => {
        observer.disconnect();
    };
};

// Theme toggle functionality
window.toggleTheme = (isDarkMode) => {
    document.body.classList.toggle('dark-mode', isDarkMode);
    localStorage.setItem('themePreference', isDarkMode ? 'dark' : 'light');
};

// Initialize theme from localStorage
window.initializeTheme = () => {
    const storedTheme = localStorage.getItem('themePreference');
    if (storedTheme === 'dark') {
        document.body.classList.add('dark-mode');
        return true;
    } else if (storedTheme === 'light') {
        document.body.classList.remove('dark-mode');
        return false;
    } else {
        // Default to dark mode if no preference is stored
        document.body.classList.add('dark-mode');
        return true;
    }
};

// Mobile sidebar toggle
window.toggleSidebar = () => {
    const sidebar = document.querySelector('.sidebar');
    sidebar.classList.toggle('show');
};