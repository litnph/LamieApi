using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lamie.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "auth_refresh_tokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    token_hash = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    revoked_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    replaced_by_token_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_by_ip = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    revoked_by_ip = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_auth_refresh_tokens", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "auth_users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    user_name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    full_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    role = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    last_login_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_auth_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cat_products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sku = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    sale_price = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    stock = table.Column<int>(type: "integer", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    thumbnail_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cat_products", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "md_categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_md_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "md_channels",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    icon_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_md_channels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "md_collections",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_md_collections", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "md_colors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    hex_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    rgb_code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_md_colors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "md_occasions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_md_occasions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "md_styles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_md_styles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "md_tags",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_md_tags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ord_order_change_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    entity_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    field_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    old_value = table.Column<string>(type: "text", nullable: true),
                    new_value = table.Column<string>(type: "text", nullable: true),
                    change_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    changed_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                    changed_by_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    changed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ord_order_change_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ord_orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    orderer_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    orderer_phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    channel_id = table.Column<Guid>(type: "uuid", nullable: false),
                    recipient_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    recipient_phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    pickup_at_shop = table.Column<bool>(type: "boolean", nullable: false),
                    delivery_address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    delivery_latitude = table.Column<double>(type: "double precision", nullable: true),
                    delivery_longitude = table.Column<double>(type: "double precision", nullable: true),
                    delivery_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deposit_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    content_note = table.Column<string>(type: "text", nullable: true),
                    shipping_fee = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    shipping_fee_actual = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    sub_total = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    payment_status = table.Column<int>(type: "integer", nullable: false),
                    order_status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ord_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sys_languages",
                columns: table => new
                {
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sys_languages", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "cat_product_images",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    image_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cat_product_images", x => x.id);
                    table.ForeignKey(
                        name: "fk_cat_product_images_cat_products_product_id",
                        column: x => x.product_id,
                        principalTable: "cat_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cat_product_translations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    language_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cat_product_translations", x => x.id);
                    table.ForeignKey(
                        name: "fk_cat_product_translations_cat_products_product_id",
                        column: x => x.product_id,
                        principalTable: "cat_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rel_product_collections",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    collection_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rel_product_collections", x => x.id);
                    table.ForeignKey(
                        name: "fk_rel_product_collections_products_product_id",
                        column: x => x.product_id,
                        principalTable: "cat_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rel_product_colors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    color_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rel_product_colors", x => x.id);
                    table.ForeignKey(
                        name: "fk_rel_product_colors_products_product_id",
                        column: x => x.product_id,
                        principalTable: "cat_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rel_product_occasions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    occasion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rel_product_occasions", x => x.id);
                    table.ForeignKey(
                        name: "fk_rel_product_occasions_cat_products_product_id",
                        column: x => x.product_id,
                        principalTable: "cat_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rel_product_styles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    style_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rel_product_styles", x => x.id);
                    table.ForeignKey(
                        name: "fk_rel_product_styles_cat_products_product_id",
                        column: x => x.product_id,
                        principalTable: "cat_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rel_product_tags",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tag_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rel_product_tags", x => x.id);
                    table.ForeignKey(
                        name: "fk_rel_product_tags_cat_products_product_id",
                        column: x => x.product_id,
                        principalTable: "cat_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "md_category_translations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    language_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_md_category_translations", x => x.id);
                    table.ForeignKey(
                        name: "fk_md_category_translations_md_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "md_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "md_collection_translations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    collection_id = table.Column<Guid>(type: "uuid", nullable: false),
                    language_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_md_collection_translations", x => x.id);
                    table.ForeignKey(
                        name: "fk_md_collection_translations_md_collections_collection_id",
                        column: x => x.collection_id,
                        principalTable: "md_collections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "md_color_translations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    color_id = table.Column<Guid>(type: "uuid", nullable: false),
                    language_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_md_color_translations", x => x.id);
                    table.ForeignKey(
                        name: "fk_md_color_translations_md_colors_color_id",
                        column: x => x.color_id,
                        principalTable: "md_colors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "md_occasion_translations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    occasion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    language_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_md_occasion_translations", x => x.id);
                    table.ForeignKey(
                        name: "fk_md_occasion_translations_md_occasions_occasion_id",
                        column: x => x.occasion_id,
                        principalTable: "md_occasions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "md_style_translations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    style_id = table.Column<Guid>(type: "uuid", nullable: false),
                    language_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_md_style_translations", x => x.id);
                    table.ForeignKey(
                        name: "fk_md_style_translations_md_styles_style_id",
                        column: x => x.style_id,
                        principalTable: "md_styles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "md_tag_translations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tag_id = table.Column<Guid>(type: "uuid", nullable: false),
                    language_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_md_tag_translations", x => x.id);
                    table.ForeignKey(
                        name: "fk_md_tag_translations_md_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "md_tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ord_order_images",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    image_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ord_order_images", x => x.id);
                    table.ForeignKey(
                        name: "fk_ord_order_images_ord_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "ord_orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ord_order_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: true),
                    product_sku = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    product_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    line_total = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ord_order_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_ord_order_items_ord_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "ord_orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_auth_refresh_tokens_token_hash",
                table: "auth_refresh_tokens",
                column: "token_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_auth_refresh_tokens_user_id",
                table: "auth_refresh_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_auth_users_email",
                table: "auth_users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_auth_users_user_name",
                table: "auth_users",
                column: "user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_cat_product_images_product_id",
                table: "cat_product_images",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_cat_product_translations_product_id_language_code",
                table: "cat_product_translations",
                columns: new[] { "product_id", "language_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_cat_products_category_id",
                table: "cat_products",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_cat_products_is_active",
                table: "cat_products",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_cat_products_sku",
                table: "cat_products",
                column: "sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_md_category_translations_category_id_language_code",
                table: "md_category_translations",
                columns: new[] { "category_id", "language_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_md_channels_code",
                table: "md_channels",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_md_collection_translations_collection_id_language_code",
                table: "md_collection_translations",
                columns: new[] { "collection_id", "language_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_md_color_translations_color_id_language_code",
                table: "md_color_translations",
                columns: new[] { "color_id", "language_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_md_occasion_translations_occasion_id_language_code",
                table: "md_occasion_translations",
                columns: new[] { "occasion_id", "language_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_md_style_translations_style_id_language_code",
                table: "md_style_translations",
                columns: new[] { "style_id", "language_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_md_tag_translations_tag_id_language_code",
                table: "md_tag_translations",
                columns: new[] { "tag_id", "language_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_ord_order_change_logs_order_id",
                table: "ord_order_change_logs",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_ord_order_change_logs_order_id_changed_at",
                table: "ord_order_change_logs",
                columns: new[] { "order_id", "changed_at" });

            migrationBuilder.CreateIndex(
                name: "ix_ord_order_images_order_id",
                table: "ord_order_images",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_ord_order_items_order_id",
                table: "ord_order_items",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_ord_orders_channel_id",
                table: "ord_orders",
                column: "channel_id");

            migrationBuilder.CreateIndex(
                name: "ix_ord_orders_delivery_at",
                table: "ord_orders",
                column: "delivery_at");

            migrationBuilder.CreateIndex(
                name: "ix_ord_orders_order_code",
                table: "ord_orders",
                column: "order_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_ord_orders_order_status",
                table: "ord_orders",
                column: "order_status");

            migrationBuilder.CreateIndex(
                name: "ix_ord_orders_orderer_phone",
                table: "ord_orders",
                column: "orderer_phone");

            migrationBuilder.CreateIndex(
                name: "ix_ord_orders_payment_status",
                table: "ord_orders",
                column: "payment_status");

            migrationBuilder.CreateIndex(
                name: "ix_ord_orders_recipient_phone",
                table: "ord_orders",
                column: "recipient_phone");

            migrationBuilder.CreateIndex(
                name: "ix_rel_product_collections_product_id_collection_id",
                table: "rel_product_collections",
                columns: new[] { "product_id", "collection_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_rel_product_colors_product_id_color_id",
                table: "rel_product_colors",
                columns: new[] { "product_id", "color_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_rel_product_occasions_product_id_occasion_id",
                table: "rel_product_occasions",
                columns: new[] { "product_id", "occasion_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_rel_product_styles_product_id_style_id",
                table: "rel_product_styles",
                columns: new[] { "product_id", "style_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_rel_product_tags_product_id_tag_id",
                table: "rel_product_tags",
                columns: new[] { "product_id", "tag_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auth_refresh_tokens");

            migrationBuilder.DropTable(
                name: "auth_users");

            migrationBuilder.DropTable(
                name: "cat_product_images");

            migrationBuilder.DropTable(
                name: "cat_product_translations");

            migrationBuilder.DropTable(
                name: "md_category_translations");

            migrationBuilder.DropTable(
                name: "md_channels");

            migrationBuilder.DropTable(
                name: "md_collection_translations");

            migrationBuilder.DropTable(
                name: "md_color_translations");

            migrationBuilder.DropTable(
                name: "md_occasion_translations");

            migrationBuilder.DropTable(
                name: "md_style_translations");

            migrationBuilder.DropTable(
                name: "md_tag_translations");

            migrationBuilder.DropTable(
                name: "ord_order_change_logs");

            migrationBuilder.DropTable(
                name: "ord_order_images");

            migrationBuilder.DropTable(
                name: "ord_order_items");

            migrationBuilder.DropTable(
                name: "rel_product_collections");

            migrationBuilder.DropTable(
                name: "rel_product_colors");

            migrationBuilder.DropTable(
                name: "rel_product_occasions");

            migrationBuilder.DropTable(
                name: "rel_product_styles");

            migrationBuilder.DropTable(
                name: "rel_product_tags");

            migrationBuilder.DropTable(
                name: "sys_languages");

            migrationBuilder.DropTable(
                name: "md_categories");

            migrationBuilder.DropTable(
                name: "md_collections");

            migrationBuilder.DropTable(
                name: "md_colors");

            migrationBuilder.DropTable(
                name: "md_occasions");

            migrationBuilder.DropTable(
                name: "md_styles");

            migrationBuilder.DropTable(
                name: "md_tags");

            migrationBuilder.DropTable(
                name: "ord_orders");

            migrationBuilder.DropTable(
                name: "cat_products");
        }
    }
}
