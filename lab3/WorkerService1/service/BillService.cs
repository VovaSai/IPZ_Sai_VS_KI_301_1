using Microsoft.EntityFrameworkCore;
using test.api;
using WorkerService1.model;

namespace WorkerService1.service;

public class BillService
{
    private readonly AppDbContext _context;
    private readonly ILogger<BillService> _logger;

    public BillService(AppDbContext context, ILogger<BillService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<(bool success, string message)> AddBill(string name, double price, string requisites, int days)
    {
        // Перевірка, чи не є поля вводу пустими
        if (string.IsNullOrWhiteSpace(name) || price <= 0 || string.IsNullOrWhiteSpace(requisites) || days <= 0)
        {
            _logger.LogWarning("Створення рахунку не вдалося. Поля вводу не можуть бути пустими.");
            return (false, "Створення рахунку не вдалося. Поля вводу не можуть бути пустими.");
        }

        // Перевірка, чи ціна більше 50
        if (price <= 50)
        {
            _logger.LogWarning("Створення рахунку не вдалося. Ціна має бути більше 50.");
            return (false, "Створення рахунку не вдалося. Ціна має бути більше 50.");
        }

        // Перевірка, чи кількість днів більше або дорівнює 7
        if (days < 7)
        {
            _logger.LogWarning("Створення рахунку не вдалося. Кількість днів має бути більше або дорівнювати 7.");
            return (false, "Створення рахунку не вдалося. Кількість днів має бути більше або дорівнювати 7.");
        }

        // Перевірка, чи requisites мають саме 12 символів
        if (requisites.Length != 12)
        {
            _logger.LogWarning("Створення рахунку не вдалося. Реквізити повинні містити рівно 12 символів.");
            return (false, "Створення рахунку не вдалося. Реквізити повинні містити рівно 12 символів.");
        }

        // Перевірка, чи requisites містять лише цифри
        if (!requisites.All(char.IsDigit))
        {
            _logger.LogWarning("Створення рахунку не вдалося. Реквізити повинні містити тільки цифри.");
            return (false, "Створення рахунку не вдалося. Реквізити повинні містити тільки цифри.");
        }

        // Перевірка, чи існує вже ім'я рахунку
        var existingBill = await _context.Bill.FirstOrDefaultAsync(b => b.Name == name);
        if (existingBill != null)
        {
            _logger.LogWarning("Рахунок з таким іменем вже існує, введіть інше ім'я.");
            return (false, "Рахунок з таким іменем вже існує, введіть інше ім'я.");
        }

        // Додавання нового рахунку
        var bill = new Bill
        {
            Name = name,
            Price = price,
            Requisites = requisites,
            Days = days,
            isPaid = false
        };

        _context.Bill.Add(bill);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Рахунок з іменем {name} було додано до бази даних.", name);
        return (true, "Рахунок успішно додано.");
    }

    public async Task<(bool success, string message)> ChangeBill(int id, string name, double price, string requisites,
        int days)
    {
        // Перевірка, чи не є поля вводу пустими
        if (string.IsNullOrWhiteSpace(name) || price <= 0 || string.IsNullOrWhiteSpace(requisites) || days <= 0)
        {
            _logger.LogWarning("Оновлення рахунку не вдалося. Поля вводу не можуть бути пустими.");
            return (false, "Оновлення рахунку не вдалося. Поля вводу не можуть бути пустими.");
        }

        // Перевірка, чи ціна більше 50
        if (price <= 50)
        {
            _logger.LogWarning("Оновлення рахунку не вдалося. Ціна має бути більше 50.");
            return (false, "Оновлення рахунку не вдалося. Ціна має бути більше 50.");
        }

        // Перевірка, чи кількість днів більше або дорівнює 7
        if (days < 7)
        {
            _logger.LogWarning("Оновлення рахунку не вдалося. Кількість днів має бути більше або дорівнювати 7.");
            return (false, "Оновлення рахунку не вдалося. Кількість днів має бути більше або дорівнювати 7.");
        }

        // Перевірка, чи requisites мають саме 12 символів
        if (requisites.Length != 12)
        {
            _logger.LogWarning("Оновлення рахунку не вдалося. Реквізити повинні містити рівно 12 символів.");
            return (false, "Оновлення рахунку не вдалося. Реквізити повинні містити рівно 12 символів.");
        }

        // Перевірка, чи requisites містять лише цифри
        if (!requisites.All(char.IsDigit))
        {
            _logger.LogWarning("Оновлення рахунку не вдалося. Реквізити повинні містити тільки цифри.");
            return (false, "Оновлення рахунку не вдалося. Реквізити повинні містити тільки цифри.");
        }

        // Знаходимо рахунок за id
        var existingBill = await _context.Bill.FirstOrDefaultAsync(b => b.Id == id);
        if (existingBill == null)
        {
            _logger.LogWarning("Рахунок з таким id не знайдено.");
            return (false, "Рахунок з таким id не знайдено.");
        }

        if (existingBill.Name == name || (int)existingBill.Price == (int)price || existingBill.Days == days ||
            existingBill.Requisites == requisites)
        {
            _logger.LogWarning("Нічого не змінилося");
            return (false, "Нічого не змінилося, щоб продовжити введіть інші дані, або відмініть редагування");
        }
        
        // Перевірка, чи ім'я не співпадає з існуючим
        var billWithSameName = await _context.Bill.FirstOrDefaultAsync(b => b.Name == name && b.Id != id);
        if (billWithSameName != null)
        {
            _logger.LogWarning("Рахунок з таким іменем вже існує.");
            return (false, "Рахунок з таким іменем вже існує.");
        }

        // Оновлення полів рахунку
        existingBill.Name = name;
        existingBill.Price = price;
        existingBill.Requisites = requisites;
        existingBill.Days = days;

        // Збереження змін в базі даних
        _context.Bill.Update(existingBill);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Рахунок з id {id} було успішно оновлено.", id);
        return (true, "Рахунок успішно оновлено.");
    }


    public async Task<(bool success, string message, List<Bill> bills)> GetBill()
    {
        _logger.LogInformation("Отримання всіх рахунків");

        try
        {
            // Отримання списку рахунків із бази даних
            var bills = await _context.Bill.ToListAsync();

            // Перевірка, чи є рахунки в базі даних
            if (bills == null || !bills.Any())
            {
                _logger.LogWarning("Жодного рахунку не знайдено.");
                return (false, "Жодного рахунку не знайдено.", new List<Bill>());
            }

            _logger.LogInformation("Рахунки успішно отримані. Знайдено {count} рахунків.", bills.Count);
            return (true, "Рахунки успішно отримані", bills);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Помилка при отриманні рахунків.");
            return (false, "Помилка при отриманні рахунків", new List<Bill>());
        }
    }

    public async Task<(bool success, string message, List<Bill> bills)> GetPaidBill()
    {
        _logger.LogInformation("Отримання всіх оплачених рахунків");

        try
        {
            // Отримання списку оплачених рахунків із бази даних
            var paidBills = await _context.Bill
                .Where(b => b.isPaid == true) // Фільтрація за IsPaid = true
                .ToListAsync();

            // Перевірка, чи є оплачувані рахунки в базі даних
            if (paidBills == null || !paidBills.Any())
            {
                _logger.LogWarning("Оплачених рахунків не знайдено.");
                return (false, "Оплачених рахунків не знайдено.", new List<Bill>());
            }

            _logger.LogInformation("Оплачені рахунки успішно отримані. Знайдено {count} рахунків.", paidBills.Count);
            return (true, "Оплачені рахунки успішно отримані", paidBills);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Помилка при отриманні оплачених рахунків.");
            return (false, "Помилка при отриманні оплачених рахунків", new List<Bill>());
        }
    }

    public async Task<(bool success, string message)> BillIsPaid(int id)
    {
        // Пошук рахунку за ID
        var bill = await _context.Bill.FirstOrDefaultAsync(b => b.Id == id);

        // Перевірка, чи знайдено рахунок
        if (bill == null)
        {
            _logger.LogWarning("Оновлення не вдалося. Рахунок з id {id} не знайдено", id);
            return (false, $"Рахунок з id {id} не знайдено");
        }

        if (bill.isPaid)
        {
            _logger.LogWarning("Рахунок з id {id} вже оплачений", id);
            return (false, $"Рахунок вже оплачений");
        }

        // Оновлення статусу isPaid на true
        bill.isPaid = true;

        // Збереження змін
        await _context.SaveChangesAsync();

        _logger.LogInformation("Рахунок з id {id} було оновлено. Статус isPaid встановлено на true", id);
        return (true, "Статус рахунку успішно оновлено на сплачено");
    }


    public async Task<(bool success, string message)> DeleteBill(int id)
    {
        // Пошук рахунку за ID
        var bill = await _context.Bill.FirstOrDefaultAsync(b => b.Id == id);

        // Перевірка, чи знайдено рахунок
        if (bill == null)
        {
            _logger.LogWarning("Видалення не вдалося. Рахунок з id {id} не знайдено", id);
            return (false, $"Рахунок з id {id} не знайдено");
        }

        // Перевірка, чи рахунок оплачений
        if (bill.isPaid)
        {
            _logger.LogWarning("Видалення не вдалося. Рахунок з id {id} вже оплачений", id);
            return (false, $"Рахунок вже оплачений і не може бути видалений");
        }

        // Видалення рахунку
        _context.Bill.Remove(bill);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Рахунок з id {id} було видалено з бази даних", id);
        return (true, "Рахунок успішно видалено");
    }
}