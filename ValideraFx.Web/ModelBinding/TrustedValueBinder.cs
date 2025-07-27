// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using Microsoft.AspNetCore.Mvc.ModelBinding;
using ValideraFx.Core;

namespace ValideraFx.Web.ModelBinding;

internal class TrustedValueBinder(
    Type innerType,
    IModelMetadataProvider metadataProvider,
    IModelBinderFactory binderFactory)
    : ValueBinderBase(typeof(TrustedValue<>), innerType, metadataProvider, binderFactory);