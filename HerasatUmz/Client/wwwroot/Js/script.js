document.addEventListener("DOMContentLoaded", function() {
    const calendar = document.getElementById('calendar');
    const todayDateDiv = document.getElementById('todayDate');
    
    let currentMonth = new Date().getMonth();
    let currentYear = new Date().getFullYear();

    // تابع برای نمایش تاریخ امروز
    function showToday() {
        const today = new Date();
        const options = { year: 'numeric', month: 'long', day: 'numeric', weekday: 'long' };
        const todayString = today.toLocaleDateString('fa-IR', options);
        todayDateDiv.innerText = `تاریخ امروز: ${todayString}`;
    }

    // تابع برای ایجاد تقویم شمسی
    function createCalendar(month, year) {
        calendar.innerHTML = `<h2>${year} - ${month + 1}</h2>`;
        
        // روزهای هفته
        const daysOfWeek = ['شنبه', 'یکشنبه', 'دوشنبه', 'سه‌شنبه', 'چهارشنبه', 'پنجشنبه', 'جمعه'];
        let weekRow = '<tr>';
        daysOfWeek.forEach(day => weekRow += `<th>${day}</th>`);
        weekRow += '</tr>';
        
        // ایجاد جدول روزها
        let daysTable = '<table><thead>' + weekRow + '</thead><tbody>';
        
        // تعداد روزهای ماه
        const daysInMonth = new Date(year, month + 1, 0).getDate();
        
        // پر کردن روزها
        for (let i = 1; i <= daysInMonth; i++) {
            if (i === 1) {
                const firstDayOfWeek = new Date(year, month, i).getDay();
                daysTable += '<tr>' + '<td></td>'.repeat(firstDayOfWeek);
            }
            
            // افزودن رویداد کلیک به هر روز
            daysTable += `<td class="day" data-day="${i}">${i}</td>`;
            
            if (new Date(year, month, i).getDay() === 6) {
                daysTable += '</tr>';
            }
        }
        
        daysTable += '</tbody></table>';
        calendar.innerHTML += daysTable;

        // افزودن رویداد کلیک برای هر روز
        document.querySelectorAll('.day').forEach(day => {
            day.addEventListener('click', function() {
                const selectedDay = this.getAttribute('data-day');
                todayDateDiv.innerText = `تاریخ انتخابی: ${selectedDay} ${month + 1} ${year}`;
            });
        });
    }

    // رویداد کلیک برای دکمه تاریخ امروز
    document.getElementById('todayButton').addEventListener('click', showToday);

    // رویداد کلیک برای دکمه ماه قبل
    document.getElementById('prevMonthButton').addEventListener('click', function() {
        if (currentMonth === 0) {
            currentMonth = 11; // دسامبر
            currentYear--;
        } else {
            currentMonth--;
        }
        createCalendar(currentMonth, currentYear);
    });

    // رویداد کلیک برای دکمه ماه بعد
    document.getElementById('nextMonthButton').addEventListener('click', function() {
        if (currentMonth === 11) {
            currentMonth = 0; // ژانویه
            currentYear++;
        } else {
            currentMonth++;
        }
        createCalendar(currentMonth, currentYear);
    });

    createCalendar(currentMonth, currentYear);
});