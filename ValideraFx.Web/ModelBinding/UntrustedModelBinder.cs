// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using Microsoft.AspNetCore.Mvc.ModelBinding;
using ValideraFx.Core;

namespace ValideraFx.Web.ModelBinding;

internal class UntrustedModelBinder(
    Type innerType,
    IModelMetadataProvider metadataProvider,
    IModelBinderFactory binderFactory)
    : ModelBinderBase(typeof(UntrustedValue<>), innerType, metadataProvider, binderFactory);