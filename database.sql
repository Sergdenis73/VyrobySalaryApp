USE vyroby_salary_db;

TRUNCATE TABLE workers_products;
TRUNCATE TABLE users;

INSERT INTO workers_products (surname, workshop_number, product_a, product_b, product_c) VALUES
('Денисенко', 1, 12, 8, 5),
('Шкурат', 1, 10, 6, 7),
('Іваненко', 2, 15, 9, 4),
('Петренко', 2, 7, 11, 6),
('Сидоренко', 3, 9, 5, 12);

INSERT INTO users (login, password, role_name) VALUES
('buhgalter', '111', 'Бухгалтер'),
('kerivnyk', '222', 'Керівник');

SELECT * FROM workers_products;
SELECT * FROM users;